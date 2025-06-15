# JKSN

A systemd service that performs various tasks.

## Overview

This application is to be run as a systemd service on debian based machines.  It runs various tasks to automate functions accross a variety of service.

## Installation

Installation can be performed manually or by an install script.  The easiest method is by install script.

The lastest release can be found [here](https://github.com/DocJ-GA/JKSN/releases/latest).

### Install Script

> [!WARNING]
> All install scripts are inheritently dangerous.
> Running this is less so, but still should be done carefully.
> We offer no garuntee or warranty that this script would not in some way adversely affect your system.

The install script will look for dotnet 9.0.0 or higher.  If it is not present, it will add the correct repository and install it.

```Directory Tree
/
├─ opt
│  └─ 🆕jksn 🧑jksn:jksn 🛡️rwxrwxr_x
│     ├─ 🆕JKSN 🧑jksn:jksn 🛡️rwxrwxr__
│     └─ 🆕appsettings.json 🧑jksn:jksn 🛡️rw_rw____
├─ etc
│  └─ 🆕jksn 🧑jksn:jksn 🛡️rwxrwx___
│     └─ 🆕config.toml 🧑jksn:jksn 🛡️rw_rw____
├─ var
│  └─ 🆕jksn 🧑jksn:jksn 🛡️rwxrwx___
└─ lib
   └─ systemd
      └─ system
         └─ 🆕jksn.service 🧑root:root 🛡️rwxrwx___
```

### Install Script: Manual

The safest way to install would be to download the [install script](https://github.com/DocJ-GA/JKSN/releases/latest/download/install.sh) install script and verify it.  The install script has been checked for an malicious code before being published and uploaded, but it is always a good idea to check through it.  It will need to be run as root to install and set up directories.

### Install Script: From Internet

> [!CAUTION]
> Running a shell or bash script from the internet is inheritently dangerious.
> You will not be able to verify the contents beforehand.
> It is even more dangerous when the script must be run as root.

The easiest way is to run the script from the internet.  This needs to be run as root.  The install script linked with the code has been vetted, and it should not cause any system wide issues.

```Console Command
sudo curl -s https://github.com/DocJ-GA/JKSN/releases/latest/download/install.sh | sudo bash
```

### Manual Installation

Manual installation is slightly more difficult but doable for anyone with a moderate amount of debian experience.  The binary package can be downloaded [here](https://github.com/DocJ-GA/JKSN/releases/download/v1.0.1/jksn-1.0.1.tar.gz).

The Directory tree for the tar.gz looks like this:

```jksn-[version].tar.gz
jksn-[version]
├─ systemd
│  └─ jksn.service
├─ config
│  └─ config.toml
└─ binarries
   └─ JKSN
   └─ appsettings.json
```

You will need to create the user and group jksn.

The final structure and permissions should look like this:

```Directory Tree
/
├─ opt
│  └─ 🆕jksn 🧑jksn:jksn 🛡️rwxrwxr_x
│     ├─ 🆕JKSN 🧑jksn:jksn 🛡️rwxrwxr__
│     └─ 🆕appsettings.json 🧑jksn:jksn 🛡️rw_rw____
├─ etc
│  └─ 🆕jksn 🧑jksn:jksn 🛡️rwxrwx___
│     └─ 🆕config.toml 🧑jksn:jksn 🛡️rw_rw____
├─ var
│  └─ 🆕jksn 🧑jksn:jksn 🛡️rwxrwx___
└─ lib
   └─ systemd
      └─ system
         └─ 🆕jksn.service 🧑root:root 🛡️rwxrwx___
```

## Tasks

The tasks that can be performed are all set in a config.toml file.  The type of tasks are below.

### Torrent

Takes a qBittorrent and gluetun url and gets the port and ensures the qBittorrent client has the port forward from gluetun set in qBittorrent.

### PingUp

Takes a url and checks to see if that service is online by trying to open an ssh connection.

## Contributes

Special thansk to Andy Jackson for contributing to this program.

## Debugging

### Windows

This application is set up so if a windows system is detected it will create the config and variable files and run it as a console app for testing and debugging.

1) The var file location is
   - `c:\ProgramData\JKSN\var`
2) The config file location is
   - `c:\ProgramData\JKSN\etc`
