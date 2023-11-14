@Library('defra-library@v-9') _

buildDotNetCore project: 'EST.MIT.Approvals.SeedProvider'

def validateClosure = {
  stage('Add Abaco container registry') {
    withCredentials([
      usernamePassword(credentialsId: 'abaco-docker-registry', usernameVariable: 'username', passwordVariable: 'password')
    ]) {
      sh("helm repo add nexus $ABACO_DOCKER_REGISTRY --username $username --password $password")
      sh('helm repo update')
    }
  }
}

buildHelm validateClosure: validateClosure