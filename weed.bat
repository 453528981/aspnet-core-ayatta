cd "F:\weed" 
rem call weed.exe server -master.port=9333 -volume.port=8080 -dir=./data 
call weed.exe server -filer=true -dir=./data -filer.redirectOnRead=true

