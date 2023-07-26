# WifiGeddan

Cross-Platform AirCrack GUI made on NET6 using the Avalonia Libraries.

# Requirements

- NpCap Driver on Windows (if not found/can be installed automatically)
- Currently GUI opens on all platforms, though actual airodump scans only run on *nix based systems
- Still in early stages of development.

# Currently implemented features:
- Switching your adapter to interface mode
- Performing AiroDump Scan and populating grids with results

# Coming Soon
- WPS Pixie Dust Attacks using Reaver & Bully
- If you have the BSSID, Pixie Dust can be done on Windows/Linux
- (just can't get airodump to work correctly in windows yet using CSharp)
  - Suppose I will need to write a wrapper? 
