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

The file: *[2026-07-21_20h46m25s_Runtime.log](./dynamic/logs/2026-07-21_20h46m25s_Runtime.log)* suggest that at **20:46:56.551**, the VM validation procedure was started and ended at **20:46:58.012**. Using this information, it is possible to identify which system information SEB queried for VM detection during this procedure. The runtime log can be found in the following folder: `C:\Users\%username%\AppData\Local\SafeExamBrowser\Logs`.


```bash
$ cat 2026-07-21_20h46m25s_Runtime.log | grep "virtual"
2026-07-21 20:46:56.551 [11] - INFO: Validating virtual machine policy...
2026-07-21 20:46:57.996 [11] - DEBUG: [VirtualMachineDetector] Computer 'THERUSTYMATE' appears to be a virtual machine (QEMU, 100%).
2026-07-21 20:46:58.012 [11] - ERROR: Detected virtual machine while SEB is not allowed to be run in a virtual machine! Aborting...
```

[ProcMon (Process Monitor)](https://learn.microsoft.com/en-us/sysinternals/downloads/procmon) can show which Windows Registry, File, SEB accessed during this time.

Searching the file: *[ProcMon_SEB.csv](./dynamic/dumps/ProcMon_SEB.csv)* will provide potential information essential to identifying system information SEB queried for VM detection:

```bash
$ cat ProcMon_SEB.csv | grep ".531"
"8:46:23.9355311 PM","SafeExamBrowser.exe","6960","Load Image","C:\Windows\System32\msvcp_win.dll","SUCCESS","Image Base: 0x7ffef0010000, Image Size: 0xa3000"
```

Using the `grep` search command: `cat ProcMon_SEB.csv | grep -E "^......56|^......57|^......58"`

```bash
$ cat ProcMon_SEB.csv | grep -E "^......56|^......57|^......58"
...
"8:46:56.9634035 PM","SafeExamBrowser.exe","6960","RegOpenKey","HKLM\System\CurrentControlSet\Enum\SWD\MMDEVAPI\MICROSOFTGSWAVETABLESYNTH","SUCCESS","Desired Access: Query Value, Maximum Allowed"
"8:46:56.9634116 PM","SafeExamBrowser.exe","6960","RegQueryValue","HKLM\System\CurrentControlSet\Enum\SWD\MMDEVAPI\MicrosoftGSWavetableSynth\HardwareID","NAME NOT FOUND","Length: 4,108"
"8:46:56.9634185 PM","SafeExamBrowser.exe","6960","RegCloseKey","HKLM\System\CurrentControlSet\Enum\SWD\MMDEVAPI\MicrosoftGSWavetableSynth","SUCCESS",""
...
```

Redirecting this output to a new modified .CSV file: *[ProcMon_SEB_modified01.csv](./dynamic/outputs/ProcMon_SEB_modified01.csv)* will allow us to inspect which system information SEB queried for VM detection with less noises.

Using the `grep` output redirection command: 

```bash
cat ProcMon_SEB.csv | grep -E "^......56|^......57|^......58" > ../outputs/ProcMon_SEB_modified01.csv
```

After removing extra data included before **20:46:56.551** and after **20:46:58.012**, it is possible to see the system information SEB queried: *[ProcMon_SEB_modified02.csv](./dynamic/outputs/ProcMon_SEB_modified02.csv)*

The custom python script: *[procmon_csv_parser.py](../src/procmon_csv_parser.py)* can parse the ProcMon CSV file and list unique items on selected columns.

```bash
$ python3 procmon_csv_parser.py -c 5 -f ../research/dynamic/outputs/ProcMon_SEB_modified02.csv
...
HKLM\System\CurrentControlSet\Services\Tcpip\Parameters
HKLM\System\CurrentControlSet\Services\Tcpip\Parameters\Domain
HKLM\System\CurrentControlSet\Services\Tcpip\Parameters\Hostname
...
```

Using the custom python script output redirection command:

```bash
$ python3 procmon_csv_parser.py -c 5 -f ../research/dynamic/outputs/ProcMon_SEB_modified02.csv > ../research/dynamic/outputs/unique_queried_sys_info_items.txt
```

*--- Ended at 2026-07-21 11:28 PM NZD ---*





# **Static Reverse Engineering**