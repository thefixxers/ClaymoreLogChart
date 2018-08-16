# ClaymoreLogChart

This is a simple program to visualize the log files created by Claymore's Eth Miner. The following charts are available at the moment:

- Hashrate/Temperature/Fan Speed for each GPU
- Avergae Hashrates/Temperatures/Fan Speeds
- Standard Deviation in Hashrate for each GPU
- Min/Max Hashrates/Temperatures/Fan Speeds
- Number of shares found for each GPU
- Number of incorrect shares for each GPU
- Number of shares found over avergae hashrate


# How to use this:
- I think you already can guess how to open a log file.
- If you want to see the hashrate/temp/fan speed values over time, press F5. A new panel opens up. Just drag and drop one or more gpus there.
- You can rename a gpu by double clicking on its name and typing a new one. These new names can be saved. By doing so, the application creates a new file in the same folder called "gpus.txt" and uses it for all the logs in that folder. So if you have logs from different rigs, it is recommended that you put them in separate folders.


# Dependencies:

- Appccelerate Event Broker (https://github.com/appccelerate/eventbroker) by Appccelerate team. 
   License: http://www.apache.org/licenses/LICENSE-2.0.html

- DockPannelSuite (http://dockpanelsuite.com/) by Weifen Luo and others.
   License: https://opensource.org/licenses/mit-license.php

- ObjectListView (http://objectlistview.sourceforge.net/cs/index.html) by Philip Piper.
   License: http://www.gnu.org/licenses/gpl.html


# Installer:

You can find the Windows Installer here: https://github.com/thefixxers/ClaymoreLogChart/releases
