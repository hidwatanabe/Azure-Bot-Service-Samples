## Create LUIS app
1. Create new LUIS app
2. Import sample data (Resources/Luis/HelpDesk1.json)
3. Train
4. Publish the app

## Create QnA Maker Knowledge bases
1. Create new knowledge base for GA and HR
2. Import sample data (Resources/QnaMaker/gaqa.txt for GA, Resources/QnaMaker/hrqa.txt for HR)
3. Save and Train
4. Publish the apps

## Set your LUIS and QnA Maker Key to App Service Settings
1. LuisAPIHostName, LuisAPIKey, LuisAppId for LUIS
2. GAQnaEndpoint, GAQnaKbId, GAQnaKey for General Affair Knowledge Base in QnA Maker
3. HRQnaEndpoint, HRQnaKbId, HRQnaKey for Human Resources Knowledge Base in QnA Maker

---
## Use Azure app service editor

1. make code change in the online editor
2. open the console window and run

```
build.cmd
```

## Use Visual Studio 

### Build and debug
1. download source code zip and extract source in local folder
2. open {PROJ_NAME}.sln in Visual Studio
3. build and run the bot
4. download and run [botframework-emulator](https://emulator.botframework.com/)
5. connect the emulator to http://localhost:3987

### Publish back

In Visual Studio, right click on {PROJ_NAME} and select 'Publish'

For first time publish after downloading source code
1. In the publish profiles tab, click 'Import'
2. Browse to 'PostDeployScripts' and pick '{SITE_NAME}.publishSettings'


## Use continuous integration

If you have setup continuous integration, then your bot will automatically deployed when new changes are pushed to the source repository.



