import java.io.File

@Library('defra-library@v-9') _

def validateClosure = {
  stage('nuget.config Transform') {
    echo "MIT_PACKAGE_FEED_URL: ${MIT_PACKAGE_FEED_URL}"
    echo "MIT_PACKAGE_FEED_USERNAME: ${MIT_PACKAGE_FEED_USERNAME}"

    echo readFile file: './EST.MIT.Approvals.SeedProvider/nuget.config'

    writeFile file: './EST.MIT.Approvals.SeedProvider/nuget.config', text: """<?xml version='1.0' encoding='utf-8'?>
        <configuration>
        <packageSources>
            <clear />
            <add key='nuget' value='https://api.nuget.org/v3/index.json' />
            <add key='DEFRA' value='${MIT_PACKAGE_FEED_URL}' />
        </packageSources>
        <packageSourceCredentials>
            <DEFRA>
            <add key='Username' value='${MIT_PACKAGE_FEED_USERNAME}' />
            <add key='ClearTextPassword' value='${MIT_PACKAGE_FEED_PAT}' />
            </DEFRA>
        </packageSourceCredentials>
        </configuration>""", encoding: "UTF-8"

    echo readFile file: './EST.MIT.Approvals.SeedProvider/nuget.config'
  }
}
buildDotNetCore project: 'EST.MIT.Approvals.SeedProvider', validateClosure: validateClosure
