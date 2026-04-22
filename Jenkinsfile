// Jenkinsfile (Windows-compatible Declarative Pipeline)

pipeline {
    agent any

    stages {

        stage('Build') {
            steps {
                echo 'Welcome'

                bat '''
                    echo My testing file has been added to test the pipeline
                    dir
                '''
            }
        }
    }
}