@Library('defra-library@v-9') _

def validateClosure = {
  stage('MIT nuget Transform') {
    echo "MIT_PACKAGE_FEED_URL: ${MIT_PACKAGE_FEED_URL}"
    echo "MIT_PACKAGE_FEED_USERNAME: ${MIT_PACKAGE_FEED_USERNAME}"
  //  echo "MIT_PACKAGE_FEED_PAT: ${MIT_PACKAGE_FEED_PAT}"

    def envFile = new File(".env")
    envFile.createNewFile()
    envFile.text="Text content"

  }
}
buildDotNetCore project: 'EST.MIT.Approvals.SeedProvider', validateClosure: validateClosure
