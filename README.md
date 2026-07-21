# EVirt_SEB
Safe Exam Browser VM Detection Bypass Research

## Disclaimer
This project and all associated materials are provided strictly for authorized penetration testing for college environments and educational and research purposes only.

This project declares that it is NOT intended to encourage cheating on online exams.

## Executive Summary
Project EVirt_SEB is a VM detection bypass research project for Safe Exam Browser (or SEB) through static and dynamic reverse engineering, ultimately to propse mitigations against identified vulnerabilities.

## Purpose
The ultimate goal of this project is to identify potential VM detection bypass-related vulnerabilities through static and dynamic reverse engineering and propse possible mitigations against identified vulnerabilities.

## Scope
The scope of this project covers [Windows Safe Exam Browser v3.10.2](https://github.com/SafeExamBrowser/seb-win-refactoring/releases/tag/v3.10.2) and will be tested on the following operating systems:

* KVM/QEMU 10.2.2
    * Windows 11 10.0.26200 Build 26200

## Out Of Scope
* MacOS Safe Exam Browser
* Exam.net
* LMS

## Tools & Environment

### Testing Environment

![Screenshot_20260721_213800](./rsrc/screenshots/Screenshot_20260721_213800.png)

| Environment           | Information                                       |
|:----------------------|:--------------------------------------------------|
| Operating System      | Fedora Linux 44 KDE Plasma Desktop Edition        |
| Virtualization Engine | QEMU emulator version 10.2.2 (qemu-10.2.2-1.fc44) |
| Architecture          | Intel x64                                         |

---

![Screenshot_20260721_212519](./rsrc/screenshots/Screenshot_20260721_212519.png)

| Item                              | Value                                 |
|:----------------------------------|:--------------------------------------|
| **OS**                            | Microsoft Windows 11 Home             |
| **OS Version**                    | 10.0.26200 Build 26200                |
| **OS Manufacturer**               | Microsoft Corporation                 |
| **System Name**                   | THERUSTYMATE                          |
| **System Manufacturer**           | QEMU                                  |
| **System Model**                  | Standard PC (Q35 + ICH9, 2009)        |
| **System Type**                   | x64-based PC                          |
| **Processor**                     | 11th Gen Intel(R) Core(TM) i5-1135G7  |
| **BIOS Version**                  | EDK II edk2-20260508-6.fc44, 5/8/2026 |
| **SMBIOS Version**                | 2.8                                   |
| **BIOS Mode**                     | UEFI                                  |
| **Platform Role**                 | Desktop                               |
| **Secure Boot State**             | On                                    |
| **Installed Physical Memory**     | 8.00 GB                               |
| **Kernel DMA Protection**         | Off                                   |
| **Virtualization-based Security** | Not enabled                           |
| **Hypervisor Bit**                | Detected                              |

*Note: this information will be changed as new VM settings applies.*

## Methodology

### Fundamental Concepts & Definitions

* [**Virtual Machine (VM)**](https://azure.microsoft.com/en-us/resources/cloud-computing-dictionary/what-is-a-virtual-machine): A virtual machine emulates a physical computer, running its own operating system and apps with virtualized resources. It’s isolated from the host system, allowing users to perform secure tasks like testing apps or using different operating systems while optimizing physical hardware.
* [**Windows Registry**](https://learn.microsoft.com/en-us/troubleshoot/windows-server/performance/windows-registry-advanced-users): A central hierarchical database used in Windows 98, Windows CE, Windows NT, and Windows 2000 used to store information that is necessary to configure the system for one or more users, applications, and hardware devices.
* [**Windows Management Instrumentation (WMI)**](https://learn.microsoft.com/en-us/windows/win32/wmisdk/wmi-start-page): Windows Management Instrumentation (WMI) is the infrastructure for management data and operations on Windows-based operating systems. Although you can write WMI scripts or applications to automate administrative tasks on remote computers, WMI also supplies management data to other parts of the operating system and products.
* [**CPUID**](https://en.wikipedia.org/wiki/CPUID): CPUID instruction (identified by a CPUID opcode) is a processor supplementary instruction (its name derived from "CPU Identification") allowing software to discover details of the processor. It was introduced by Intel in 1993 with the launch of the Pentium and late 486 processors.

---

* **SEB**: Safe Exam Browser


### Techniques & Strategies
**SEB VM detection techniques are based on Windows Registry, WMI, CPUID, Hardware Enumeration, Driver Enumeration**. As spoofing these components are possible, **it is possible to decrease the VM detection confidence by spoofing the system components above, resulting possible VM detection bypass**.

### Validation Method

### Validation Standards

### Validation Assumption

## Research
[Reverse Engeering Research](./research/README.md)

## Validation

## Limitations

## References
ChatGPT, Gemini, and DeepSeek were used to improve the technical English expressions in this document.
* [https://safeexambrowser.org/download_en.html](https://safeexambrowser.org/download_en.html)
* [https://github.com/SafeExamBrowser/seb-win-refactoring/releases/tag/v3.10.2](https://github.com/SafeExamBrowser/seb-win-refactoring/releases/tag/v3.10.2)
* [https://en.wikipedia.org/wiki/Virtual_machine](https://en.wikipedia.org/wiki/Virtual_machine)
* [https://learn.microsoft.com/en-us/windows/win32/wmisdk/wmi-start-page](https://learn.microsoft.com/en-us/windows/win32/wmisdk/wmi-start-page)
* [https://en.wikipedia.org/wiki/Windows_Registry](https://en.wikipedia.org/wiki/Windows_Registry)