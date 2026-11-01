pipeline{
    agent any
    
    environment{
        SONAR_HOME = tool 'sonar-scanner'
        Image_tag = "v${BUILD_NUMBER}"
    }
    stages{
        stage('Git Checkout'){
            steps{
                git branch: 'main', url: 'https://github.com/SurajKhanderao/Capstone-DotNET-Mongo-CI.git'
            }
        }
        stage('GitLeaks'){
            steps{
                sh 'gitleaks detect --report-format=json --report-path=gitleaks-report.json --exit-code=1'
            }
        }
        stage('Compile'){
            steps{
                sh 'dotnet build'
            }
        }
        stage('Trivy file Scan'){
            steps{
                sh 'trivy fs --format table -o fs-report.html .'
            }
        }
        stage('Test'){
            steps{
                echo "Unit Tests"
            }
        }
        stage('SonarQube ANalysis'){
            steps{
                withSonarQubeEnv('sonar-server') {
                     sh ''' 
                            $SONAR_HOME/bin/sonar-scanner \
                            -Dsonar.organization=surajkhanderao \
                            -Dsonar.projectKey=surajkhanderao_sonar-project
                       '''
                }
            }
        }
      stage('Quality Gate Check') {
            steps {
                timeout(time: 1, unit: 'HOURS') {
                     waitForQualityGate abortPipeline: false, credentialsId: 'sonar-token'
                    }
            }
        }
        stage('Docker Image Build'){
            steps{
              script{
                withDockerRegistry(credentialsId: 'docker-hub-cred') {
                   sh "docker build -t surajkhanderao/noteapp:${Image_tag} ."
                }
              }
            }
        }
        stage('Trivy Image Scan'){
            steps{
                sh 'trivy image --format table -o image-report.html surajkhanderao/noteapp:${Image_tag}'
            }
        }
         stage('Docker Image Push'){
            steps{
              script{
                withDockerRegistry(credentialsId: 'docker-hub-cred') {
                   sh "docker push surajkhanderao/noteapp:${Image_tag}"
                }
              }
            }
        }
         stage('Update Image_Tag in Manifest'){
            steps{
                  script{
                          cleanWs()
                         withCredentials([usernamePassword(credentialsId: 'git-cred', passwordVariable: 'GIT_PASSWORD', usernameVariable: 'GIT_USERNAME')]) {
                              sh '''
                              git clone https://${GIT_USERNAME}:${GIT_PASSWORD}@github.com/SurajKhanderao/Capstone-DotNET-Mongo-CD.git
                                 
                                 cd Capstone-DotNET-Mongo-CD
                                 
                                 sed -i "s|surajkhanderao/noteapp:.*|surajkhanderao/noteapp:${Image_tag}|" Manifest/manifest.yaml
                                 
                                 echo " Manifest Image Updated with Tag ${Image_tag}"
                                 
                                  cat Manifest/manifest.yaml
                                  
                                   # Commit and push the changes
                                      git config user.name "Jenkins"
                                      git config user.email "jenkins@example.com"
                                      git add Manifest/manifest.yaml
                                      git commit -m "Update image tag to ${IMAGE_TAG}"
                                     git push origin main
                        '''
                        }
                  }
            }
        }
       
    }
    post {
    always {
        script {
            def jobName = env.JOB_NAME
            def buildNumber = env.BUILD_NUMBER
            def pipelineStatus = currentBuild.result ?: 'UNKNOWN'
            def bannerColor = pipelineStatus.toUpperCase() == 'SUCCESS' ? 'green' : 'red'

            def body = """
                <html>
                <body>
                <div style="border: 4px solid ${bannerColor}; padding: 10px;">
                <h2>${jobName} - Build ${buildNumber}</h2>
                <div style="background-color: ${bannerColor}; padding: 10px;">
                <h3 style="color: white;">Pipeline Status: ${pipelineStatus.toUpperCase()}</h3>
                </div>
                <p>Check the <a href="${BUILD_URL}">console output</a>.</p>
                </div>
                </body>
                </html>
            """

            emailext (
                subject: "${jobName} - Build ${buildNumber} - ${pipelineStatus.toUpperCase()}",
                body: body,
                to: 'khanderaongn@gmail.com',
                from: 'khanderaosurj@gmail.com',
                replyTo: 'jenkins@devopsshack.com',
                mimeType: 'text/html',
               
            )
        }
    }
}
}
