using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

public sealed class EVirtPatcher
{
    private static readonly string LOGO_ASCII = @"""
 ██████████ █████   █████  ███             █████     
░░███░░░░░█░░███   ░░███  ░░░             ░░███      
 ░███  █ ░  ░███    ░███  ████  ████████  ███████    
 ░██████    ░███    ░███ ░░███ ░░███░░███░░░███░     
 ░███░░█    ░░███   ███   ░███  ░███ ░░░   ░███      
 ░███ ░   █  ░░░█████░    ░███  ░███       ░███ ███  
 ██████████    ░░███      █████ █████      ░░█████   
░░░░░░░░░░      ░░░      ░░░░░ ░░░░░        ░░░░░    
                                                     
                                                     
                                                     
 ███████████             █████             █████     
░░███░░░░░███           ░░███             ░░███      
 ░███    ░███  ██████   ███████    ██████  ░███████  
 ░██████████  ░░░░░███ ░░░███░    ███░░███ ░███░░███ 
 ░███░░░░░░    ███████   ░███    ░███ ░░░  ░███ ░███ 
 ░███         ███░░███   ░███ ███░███  ███ ░███ ░███ 
 █████       ░░████████  ░░█████ ░░██████  ████ █████
░░░░░         ░░░░░░░░    ░░░░░   ░░░░░░  ░░░░ ░░░░░ 
""";
    private static readonly string DISCLAIMER_TEXT = 
        "[ DISCLAIMER ]\n" + 
        "This project and all associated materials are provided strictly for authorized penetration " +
        "testing for college environments and educational and research purposes only.\n\n" +
        "This project declares that it is NOT intended to encourage cheating on online exams.";

    private static Dictionary<string, ModuleDefMD> loadedModules = new();

    private static readonly string BASE_PATH = @"C:\Program Files\SafeExamBrowser\Application";

    // -----------------------------------------------------------------------

    private static readonly List<(string TARGET, string CLASS_NAME, string METHOD_NAME, bool patchRet)> SEB_PATCH_TARGETS = new()
    {
        ("SafeExamBrowser.Monitoring.dll", "SafeExamBrowser.Monitoring.VirtualMachineDetector", "IsVirtualMachine", false),
        ("SafeExamBrowser.Monitoring.dll", "SafeExamBrowser.Monitoring.RemoteSessionDetector", "IsRemoteSession", false),
    };

    // -----------------------------------------------------------------------

    public static int Main()
    {
        Console.WriteLine(LOGO_ASCII); 
        Console.WriteLine(DISCLAIMER_TEXT); 
        Console.WriteLine();

        try
        {
            bool isPatched = true;

            // Group patches by target file to avoid loading same file multiple times
            var groupedPatches = new Dictionary<string, List<(string className, string methodName, bool patchRet)>>();
            
            foreach ((string target, string className, string methodName, bool patchRet) in SEB_PATCH_TARGETS)
            {
                if (!groupedPatches.ContainsKey(target))
                {
                    groupedPatches[target] = new List<(string, string, bool)>();
                }
                groupedPatches[target].Add((className, methodName, patchRet));
            }

            foreach (var kvp in groupedPatches)
            {
                string target = kvp.Key;
                var patches = kvp.Value;

                string targetPath = Path.Combine(BASE_PATH, target);
                string backupPath = Path.Combine(
                    BASE_PATH, 
                    Path.GetFileNameWithoutExtension(target) + "_backup.dll"
                );

                if (!File.Exists(targetPath))
                {
                    LogError($"Patch target: [{target}] is not found.");
                    isPatched = false;
                    continue;
                }

                // Create backup before any modifications (only if backup doesn't exist)
                if (!File.Exists(backupPath))
                {
                    try
                    {
                        File.Copy(targetPath, backupPath, overwrite: false);
                        LogInfo($"Backup created: {Path.GetFileName(backupPath)}");
                    }
                    catch (Exception ex)
                    {
                        LogError($"Failed to create backup for {target}: {ex.Message}");
                        isPatched = false;
                        continue;
                    }
                }
                else
                {
                    LogInfo($"Backup already exists: {Path.GetFileName(backupPath)}");
                }

                // Load module once per file
                ModuleDefMD? module = null;
                try
                {
                    // Load from the backup or original file
                    module = ModuleDefMD.Load(targetPath);
                    LogInfo($"Successfully loaded: {target}");

                    bool allPatchesSucceeded = true;
                    
                    foreach (var (className, methodName, patchRet) in patches)
                    {
                        if (Patch(module, className, methodName, patchRet))
                        {
                            LogInfo($"Patch applied: [{className}::{methodName}]");
                        }
                        else
                        {
                            LogError($"Patch Failed: [{className}::{methodName}] at {target}");
                            allPatchesSucceeded = false;
                            isPatched = false;
                        }
                    }

                    // Write all patches at once to a temporary file first
                    if (allPatchesSucceeded)
                    {
                        string tempPath = targetPath + ".tmp";
                        
                        try
                        {
                            // Write to temporary file
                            module.Write(tempPath);
                            module.Dispose();
                            module = null;

                            // Replace original with patched version
                            File.Delete(targetPath);
                            File.Move(tempPath, targetPath);
                            
                            LogInfo($"Successfully patched: {target}");
                        }
                        catch (Exception ex)
                        {
                            LogError($"Failed to write patched file {target}: {ex.Message}");
                            isPatched = false;
                            
                            // Clean up temp file if it exists
                            if (File.Exists(tempPath))
                            {
                                try { File.Delete(tempPath); } catch { }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Failed to process {target}: {ex.Message}");
                    isPatched = false;
                }
                finally
                {
                    module?.Dispose();
                }
            }

            if (isPatched)
            {
                LogInfo("SafeExamBrowser is now fully patched.");
            }
            else
            {
                LogError("Patch targets are not fully patched.");
            }
            
            Console.ReadKey(); 

            return isPatched ? 0 : 1;
        }
        catch (Exception ex)
        {
            LogCritical($"Fatal error: {ex}");
            return 1;
        }
    }

    public static bool Patch(
        ModuleDefMD module,
        string className,
        string methodName,
        bool returnVar
    )
    {
        TypeDef? targetType = null;

        foreach (TypeDef type in module.GetTypes())
        {
            if (type.FullName == className)
            {
                targetType = type;
                break;
            }
        }

        if (targetType == null)
        {
            LogError($"Type not found: {className}");
            return false;
        }

        foreach (MethodDef method in targetType.Methods)
        {
            if (method.Name != methodName)
                continue;

            if (!method.HasBody)
            {
                LogError(
                    $"Method has no IL body: {className}::{methodName}"
                );
                return false;
            }

            CilBody body = method.Body;

            body.Instructions.Clear();
            body.ExceptionHandlers.Clear();
            body.Variables.Clear();

            body.InitLocals = false;
            body.MaxStack = 1;

            body.Instructions.Add(
                returnVar
                    ? Instruction.Create(OpCodes.Ldc_I4_1)
                    : Instruction.Create(OpCodes.Ldc_I4_0)
            );

            body.Instructions.Add(
                Instruction.Create(OpCodes.Ret)
            );

            return true;
        }

        LogError(
            $"Method not found: {className}::{methodName}"
        );

        return false;
    }

    public static void LogInfo(string msg)      { Console.WriteLine($"[+] {msg}"); }
    public static void LogError(string msg)     { Console.WriteLine($"[-] {msg}"); }
    public static void LogWarning(string msg)   { Console.WriteLine($"[!] {msg}"); }
    public static void LogCritical(string msg)  { Console.WriteLine($"[C] {msg}"); }
}