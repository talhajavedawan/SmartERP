// Jenkinsfile (Declarative Pipeline)

pipeline {
    agent {
        docker {
            image 'maven:3.9.0-eclipse-temurin-11'
        }
    }

    stages {
        stage('Build') {
            steps {
                sh 'mvn --version'
            }
        }
    }
}
