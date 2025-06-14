#!/bin/bash
if [[ $EUID -ne 0 ]]; then
   echo "This script must be run as root." 
   exit 1
fi

version_check () {
    if [[ ! $(command -v dotnet >/dev/null 2>&1) ]]; then
        return 0
    fi
    version=$(dotnet --version)
    printf '%s\n%s\n' "9.0.0" "$version" | sort --check=quiet --version-sort
}

make_dir () {
    echo "Creating directory $1"
    mkdir "$1"
}

update_permissions () {
    echo "Settings permissions for $1"
    echo "Setting owner to jksn."
    chown jksn: -R "$1"
    echo "Adding read permissions for all."
    chmod a+r -R "$1"
    echo "Removing write permissions for other."
    chmod o-w -R "$1"
    echo "Adding write permissions to jksn:jksn."
    chmod ug+w -R "$1"
    echo "Adding executable to folders for all."
    chmod a+X -R "$1"
}

echo "Looking for dotnet."
if ! version_check
then
    echo "dotnet version adaquate."
else
    echo "SDK dotnet not found or version is not 9.0.0 or higher."
    source /etc/lsb-release
    if [[ "$DISTRIB_RELEASE" == "24.04" || "$DISTRIB_RELEASE" == "22.04" ]]; then
        echo "Ubuntu $DISTRIB_RELEASE $DISTRIB_CODENAME detected, installing dotnet 9.0 SDK."
        echo "Adding repository."
        add-apt-repository ppa:dotnet/backports
        echo "repository added."
    elif [[ "$DISTRIB_RELEASE" == "24.10" ]]; then
        echo "Adding repository."
        echo "Ubuntu $DISTRIB_RELEASE $DISTRIB_CODENAME detected, installing dotnet 9.0 SDK."
        echo "Downloading the repository package."
        curl -sSL -O https://packages.microsoft.com/config/ubuntu/"$DISTRIB_RELEASE"/packages-microsoft-prod.deb
        echo "Download complete."
        echo "Deploying the repository package."
        dpkg -i packages-microsoft-prod.deb
        echo "Repository package deployed."
        echo "Removing download."
        rm packages-microsoft-prod.deb
        echo "Download removed."
    else
        echo "Unsupported Ubuntu version: $DISTRIB_RELEASE $DISTRIB_CODENAME"
        exit 1
    fi

    echo "Updating apt repository."
    apt-get update
    echo "Apt repository updated."
    echo "Install dotnet 9.0 SDK"
    apt install -y dotnet-sdk-9.0
    echo "Installed"
fi

echo "Checking for user jksn."
if getent passwd | grep -c '^username:'; then
    echo "User jksn already exists."
else
    echo "Creating user jksn."
    useradd -M jksn
    usermod -L jskn
    echo "User jksn created."
fi

echo "Downloading the latest jksn release from GitHub."
wget https://github.com/DocJ-GA/JKSN/releases/download/v1.0.0/jksn-1.0.0.tar.gz
echo "Extracting jksn binarries."
tar -xzf jksn-1.0.0.tar.gz
echo "Removing compressed archive."
rm jksn-1.0.0.tar.gz

echo "Installing jksn."

make_dir "/var/jksn"
make_dir "/etc/jksn"
make_dir "/opt/jksn"

echo "Copying binarries into '/opt/jksn'."
cp jksn-1.0.0/binaries/* /opt/jksn/
echo "Creating symlink into '/usr/bin'."
ln -s /opt/jksn/JKSN /usr/bin/JKSN

echo "Adding executable permissions for jksn:jksn and removing for other."
chmod ug+x /opt/jksn/JKSN
chmod o-x /opt/jksn/JKSN

echo "Creating initial configuration file".
cp jksn-1.0.0/config/config.toml /etc/jksn/config.toml

update_permissions "/opt/jksn"
update_permissions "/etc/jksn"
update_permissions "/var/jksn"

echo "Creating systemd file."
cp jksn-1.0.0/systemd/jksn.service /lib/systemd/system/jksn.service

echo "Updating daemon."
systemctl daemon-reload

echo "Enabling service 'jksn'."
systemctl enable jksn

echo "Starting service."
service jksn start

echo "Removing intall files."
rm -r jksn-1.0.0
echo "Installation finished."