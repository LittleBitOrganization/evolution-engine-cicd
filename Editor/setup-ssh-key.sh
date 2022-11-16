#!/usr/bin/env bash
echo "${SSH_PRIVATE_KEY}" | tr -d '\r' > /tmp/ssh_key
chmod 600 /tmp/ssh_key
mkdir -p ~/.ssh
eval `ssh-agent -s`
ssh-add /tmp/ssh_key
ssh-add -L