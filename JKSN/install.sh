#!/bin/bash
if [[ $EUID -ne 0 ]]; then
   echo "This script must be run as root." 
   exit 1
fi

version_check () {
    version=$(dotnet --version)
    printf '%s\n%s\n' "9.0.0" "$version" | sort --check=quiet --version-sort
}

echo "Looking for dotnet."
if ! version_check
then
    echo "dotnet version adaquate."
else
    echo "SDK dotnet not found or version is not 9.0.0 or higher."
    source /etc/lsb-release
    if [[ "$DISTRIB_RELEASE" == "24.04" || "$DISTRIB_RELEASE" == "22.04"]]; then
        echo "Ubuntu $DISTRIB_RELEASE $DISTRIB_CODENAME detected, installing dotnet 9.0 SDK."
        echo "Adding repository."
        add-apt-repository ppa:dotnet/backports
        echo "repository added."
    elif [[ "$DISTRIB_RELEASE" -eq "24.10" ]]; then
        echo "Adding repository."
        echo "Ubuntu $DISTRIB_RELEASE $DISTRIB_CODENAME detected, installing dotnet 9.0 SDK."
        echo "Downloading the repository package."
        curl -sSL -O https://packages.microsoft.com/config/ubuntu/$DISTRIB_RELEASE/packages-microsoft-prod.deb
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

echo "Creating '/var/jksn' directory."
mkdir /var/jksn
echo "Creating '/etc/jksn' directory."
mkdir /etc/jksn
echo "Creating '/opt/jksn' directory.";
mkdir /opt/jksn

echo "Copying binarries into '/opt/jksn'."
cp jksn-1.0.0/binaries/* /opt/jksn/
echo "Creating symlink into '/usr/bin'."
ln -s /opt/jksn/JKSN /usr/bin/JKSN

echo "Adding executable permissions for jksn."
chmod o+x /opt/jksn/JKSN
chmod g+x /opt/jksn/JKSN

echo "Creating initial configuration file".
cp jksn-1.0.0/config/config.toml /etc/jksn/config.toml

echo "Changing ownership of files to 'jksn:jksn'."
chown -R jksn:jksn /opt/jksn
chown -R jksn:jksn /etc/jksn
chown -R jksn:jksn /var/jksn

echo "Setting permissions for '/var/jksn' to 770:660."
chmod -R u-rwx /opt/jksn
chmod -R u-rwx /etc/jksn
chmod -R u-rwx /var/jksn

echo "Creating systemd file."
cp systemd/jksn.service /lib/systemd/system/jksn.service

echo "Updating daemon."
systemctl daemon-reload

echo "Enabling service 'jksn'."
systemctl enable jksn

echo "Starting service."
service jksn start

echo "Installation finished."