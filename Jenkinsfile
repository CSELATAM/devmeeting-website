pipeline {
  agent {
    docker {
      image 'microsoft/dotnet:2.0-sdk'
    }

  }
  stages {
    stage('build') {
      steps {
        sh '''dotnet restore
dotnet build
'''
      }
    }
  }
}