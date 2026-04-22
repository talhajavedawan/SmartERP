// Jenkinsfile (Windows-compatible Declarative Pipeline)

pipeline {
    agent any

    stages {

        stage('Build') {
            steps {
                echo 'Welcome'

                bat '''
                    echo The multiline shell steps are working
                    dir
                '''
            }
        }
    }
}