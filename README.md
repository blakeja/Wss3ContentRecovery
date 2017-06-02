# WSS 3.0 Content Recovery

Original code and details can be found here:

https://music.krichie.com/2008/07/06/exporting-site-content-from-a-sharepoint-content-database-for-recovery-purposes/

## Download

https://github.com/blakeja/Wss3ContentRecovery/releases/latest

## Quick Help

### Arguments

-server (Required)

-database (Required)

-connectiontimeout (Optional, default = 30)

-commandtimeout (Optional, default = 30)

-buffersize (Optional, default = 1000000)

-whatif (Optional, default = false)


### Examples

recover.exe -server your_server_name -database your_database_name

recover.exe -server your_server_name -database your_database_name -whatif

recover.exe -server your_server_name -database your_database_name -connectiontimeout 60 -commandtimeout 45

Specifying -whatif will only test the database connection and check file name length. Directory creation and file writing will be skipped.
