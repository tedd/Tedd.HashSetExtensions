#!/bin/bash
# Simulating azure environment by attempting a restore using a fake or standard environment.
# Actually azure might be failing because we are using net10.0 which isn't installed by default on Azure's ubuntu-latest unless setup.
# The github workflow sets up dotnet 10.0.x, 8.0.x, 6.0.x. Azure pipeline does NOT setup dotnet.
