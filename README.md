# GitStat
**GitStat** is a small CLI utility which can be used to gather and compare information about releases in Github repositories 
### Example
``` Batch
------------------------------------------------------
GitStat

Otuput:

Usage: GitStat <repo-author> <repo-name> [-s] [-C <path>] [-S <path>]

Help:
[-C <path>] - Compare current repo release status to an older one which is saved in a json file
[-S <path>] - Save current repo release status to a file
[-s] - Silent run, without writing to console except errors
------------------------------------------------------
```
``` Batch
------------------------------------------------------
GitStat murkyyt csauto -C CSAuto.json -S CSAuto.json -s

Otuput:

Overall download count 221
Nothing has changed!
Succesfully saved data to CSAuto.json
------------------------------------------------------
```
