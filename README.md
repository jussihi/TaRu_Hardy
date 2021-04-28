# TaRu Hardy controller

*A better solution for controlling Hardy targets.*

## Changelog

TBD

### What works:

 - Target selection (address mapping), with possibility to select all/none
 - Basic "one shot" commands with no target response: UP, DOWN, RESET, SET SENSITIVITY, SET HITS TO FALL, LIGHTS ON/OFF, MOTION ON/OFF
 - Quick programming with setup of: hits to fall, target up, target down, start and end program
 - The above commands with both arbitrary selected targets or with all targets (special command for all targets)
 - Script manager! (saving, loading, creating and running of scripts)
 - Initial COM port connection
 - Rudimentary logging system
 - Fetching of target configuration, shown on the preferences tab as a list

### What does not work:

 - COM port reconnection in case of changed port/failed connection
 - Fetchable command HITS not yet implemented

### Future functionality:

 - Show COM port status with a GREEN/YELLOW/RED light (Connected, Busy writing/reading, Disconnected)
 - Line numbers to script editor
 - Something else?