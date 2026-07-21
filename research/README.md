<div align="center">
    <h1>Project EVirt_SEB</h1>
    <h3>Safe Exam Browser VM Detection Bypass Research</h3>
</div>

---

# **Process Execution Flow**
1. Initialize startup procedure
    * Load `system language`
2. Start communication host
    * Open a net pipe: `net.pipe://localhost/safeexambrowser/runtime/[UUID]`
3. Verify application and runtime integrity
4. Initialize new session configuration
    * Load `Client-ID`
    * Load `Runtime-ID`
    * Load `Session-ID`
5. Initialize application configuration
6. Validate version restrictions
7. Validate remote session policy
8. Validate session integrity
    * Disable sticky keys
    * Start cursor verification
    * Start ease of access verification
9. **Validate virtual machine policy**
10. Continue OR Exit


# **Dynamic Reverse Engineering**

The file: *[2026-07-21_20h46m25s_Runtime.log](./dynamic/logs/2026-07-21_20h46m25s_Runtime.log)* suggest that at **20:46:56.551**, the VM validation procedure was started and ended at **20:46:58.012**. Using this information, it is possible to identify which componenets SEB accessed during this procedure.


```bash
$ cat 2026-07-21_20h46m25s_Runtime.log | grep "virtual"
2026-07-21 20:46:56.551 [11] - INFO: Validating virtual machine policy...
2026-07-21 20:46:57.996 [11] - DEBUG: [VirtualMachineDetector] Computer 'THERUSTYMATE' appears to be a virtual machine (QEMU, 100%).
2026-07-21 20:46:58.012 [11] - ERROR: Detected virtual machine while SEB is not allowed to be run in a virtual machine! Aborting...
```

[ProcMon (Process Monitor)](https://learn.microsoft.com/en-us/sysinternals/downloads/procmon) can show which Windows Registry, File, SEB accessed during this time.




# **Static Reverse Engineering**