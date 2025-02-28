//#include <glad/glad.h>
//#include <GLFW/glfw3.h>
//#include <iostream>
//#include <cmath>
//#include <vector>
//#include <glm/glm.hpp>
//#include <glm/gtc/matrix_transform.hpp>
//#include <glm/gtc/type_ptr.hpp>
//#include <stb_image.h>
//
//#define STB_IMAGE_IMPLEMENTATION
//#include "stb_image.h"
//
//// Definiuj sta³¹ M_PI, jeœli nie jest zdefiniowana
//#ifndef M_PI
//#define M_PI 3.14159265358979323846
//#endif
//
//void framebuffer_size_callback(GLFWwindow* window, int width, int height);
//void processInput(GLFWwindow* window);
//void mouse_callback(GLFWwindow* window, double xpos, double ypos);
//void scroll_callback(GLFWwindow* window, double xoffset, double yoffset);
//unsigned int loadTexture(const char* path);
//
//// settings
//const unsigned int SCR_WIDTH = 1200;  // Powiêkszone okno
//const unsigned int SCR_HEIGHT = 800;
//
//// Kamery
//enum CameraMode { STATIC, FOLLOW_CAR, CAR_FIRST_PERSON };
//CameraMode currentCameraMode = STATIC;
//glm::vec3 cameraPos = glm::vec3(0.0f, 2.0f, 5.0f);
//glm::vec3 cameraFront = glm::vec3(0.0f, 0.0f, -1.0f);
//glm::vec3 cameraUp = glm::vec3(0.0f, 1.0f, 0.0f);
//float yaw = -90.0f, pitch = 0.0f;
//float lastX = SCR_WIDTH / 2.0f, lastY = SCR_HEIGHT / 2.0f;
//bool firstMouse = true;
//
//// Timing
//float deltaTime = 0.0f;
//float lastFrame = 0.0f;
//
//// Œwiat³a
//glm::vec3 pointLightPos = glm::vec3(-1.2f, 1.0f, -2.0f); // Nieruchome Ÿród³o œwiat³a
//glm::vec3 pointLightColor = glm::vec3(1.0f, 1.0f, 1.0f);
//glm::vec3 spotlightDir = glm::vec3(0.0f, 0.0f, -1.0f);
//float spotlightCutOff = glm::cos(glm::radians(12.5f));
//
//// Nowe œwiat³o statyczne dla piramidy
//glm::vec3 staticPyramidLightPos = glm::vec3(6.0f, 2.0f, -2.0f); // Pozycja œwiat³a
//glm::vec3 staticPyramidLightColor = glm::vec3(1.0f, 1.0f, 0.8f); // Kolor œwiat³a (jasno¿ó³ty)
//
//// Poruszaj¹cy siê samochód
//float carPosX = 0.0f;
//float carSpeed = 2.5f;
//
//// Mg³a
//float fogDensity = 0.06f; // Zwiêkszona gêstoœæ mg³y
//bool isDay = true;
//
//// Shadery
//const char* vertexShaderSource = "#version 330 core\n"
//"layout (location = 0) in vec3 aPos;\n"
//"layout (location = 1) in vec3 aNormal;\n"
//"layout (location = 2) in vec2 aTexCoords;\n"
//"layout (location = 3) in vec3 aTangent;\n"
//"layout (location = 4) in vec3 aBitangent;\n"
//"out vec3 FragPos;\n"
//"out vec2 TexCoords;\n"
//"out vec3 TangentLightPos;\n"
//"out vec3 TangentViewPos;\n"
//"out vec3 TangentFragPos;\n"
//"uniform mat4 model;\n"
//"uniform mat4 view;\n"
//"uniform mat4 projection;\n"
//"uniform vec3 lightPos;\n"
//"uniform vec3 viewPos;\n"
//"void main()\n"
//"{\n"
//"   FragPos = vec3(model * vec4(aPos, 1.0));\n"
//"   TexCoords = aTexCoords;\n"
//"   vec3 T = normalize(vec3(model * vec4(aTangent, 0.0)));\n"
//"   vec3 B = normalize(vec3(model * vec4(aBitangent, 0.0)));\n"
//"   vec3 N = normalize(vec3(model * vec4(aNormal, 0.0)));\n"
//"   mat3 TBN = transpose(mat3(T, B, N));\n"
//"   TangentLightPos = TBN * lightPos;\n"
//"   TangentViewPos  = TBN * viewPos;\n"
//"   TangentFragPos  = TBN * FragPos;\n"
//"   gl_Position = projection * view * model * vec4(aPos, 1.0);\n"
//"}\0";
//
//const char* fragmentShaderSource = "#version 330 core\n"
//"out vec4 FragColor;\n"
//"in vec3 FragPos;\n"
//"in vec2 TexCoords;\n"
//"in vec3 TangentLightPos;\n"
//"in vec3 TangentViewPos;\n"
//"in vec3 TangentFragPos;\n"
//"uniform vec3 viewPos;\n"
//"uniform vec3 objectColor;\n"
//"uniform vec3 pointLightPos;\n"
//"uniform vec3 pointLightColor;\n"
//"uniform vec3 spotlightDir;\n"
//"uniform vec3 spotlightColor;\n"
//"uniform float spotlightCutOff;\n"
//"uniform sampler2D texture_diffuse1;\n"
//"uniform sampler2D normalMap;\n"
//"uniform float fogDensity;\n"
//"uniform bool isDay;\n"
//"uniform vec3 staticPyramidLightPos;\n"
//"uniform vec3 staticPyramidLightColor;\n"
//"void main()\n"
//"{\n"
//"   // Obtain normal from normal map in range [0,1]\n"
//"   vec3 normal = texture(normalMap, TexCoords).rgb;\n"
//"   normal = normalize(normal * 2.0 - 1.0);  // Transform normal vector to range [-1,1]\n"
//"   \n"
//"   // Obtain light and view vectors in tangent space\n"
//"   vec3 lightDir = normalize(TangentLightPos - TangentFragPos);\n"
//"   vec3 viewDir = normalize(TangentViewPos - TangentFragPos);\n"
//"   \n"
//"   // Ambient\n"
//"   float ambientStrength = 0.1;\n"
//"   vec3 ambient = ambientStrength * pointLightColor;\n"
//"   \n"
//"   // Diffuse\n"
//"   float diff = max(dot(normal, lightDir), 0.0);\n"
//"   vec3 diffuse = diff * pointLightColor;\n"
//"   \n"
//"   // Specular\n"
//"   float specularStrength = 0.5;\n"
//"   vec3 reflectDir = reflect(-lightDir, normal);\n"
//"   float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);\n"
//"   vec3 specular = specularStrength * spec * pointLightColor;\n"
//"   \n"
//"   // Spotlight\n"
//"   vec3 spotDir = normalize(-spotlightDir);\n"
//"   float theta = dot(lightDir, spotDir);\n"
//"   if (theta > spotlightCutOff) {\n"
//"       vec3 spotlight = (diff + specular) * spotlightColor;\n"
//"       diffuse += spotlight;\n"
//"   }\n"
//"   \n"
//"   // Nowe œwiat³o statyczne (dla piramidy)\n"
//"   vec3 staticLightDir = normalize(staticPyramidLightPos - FragPos);\n"
//"   float staticDiff = max(dot(normal, staticLightDir), 0.0);\n"
//"   vec3 staticDiffuse = staticDiff * staticPyramidLightColor;\n"
//"   vec3 staticReflectDir = reflect(-staticLightDir, normal);\n"
//"   float staticSpec = pow(max(dot(viewDir, staticReflectDir), 0.0), 32);\n"
//"   vec3 staticSpecular = specularStrength * staticSpec * staticPyramidLightColor;\n"
//"   \n"
//"   vec3 result = (ambient + diffuse + specular + staticDiffuse + staticSpecular) * objectColor;\n"
//"   FragColor = texture(texture_diffuse1, TexCoords) * vec4(result, 1.0);\n"
//"   \n"
//"   // Fog\n"
//"   float distance = length(FragPos - viewPos);\n"
//"   float fogFactor = exp(-pow(fogDensity * distance, 2.0));\n"
//"   fogFactor = clamp(fogFactor, 0.0, 1.0);\n"
//"   vec3 fogColor = isDay ? vec3(0.5f, 0.5f, 0.5f) : vec3(0.1f, 0.1f, 0.1f);\n"
//"   FragColor = mix(vec4(fogColor, 1.0), FragColor, fogFactor);\n"
//"}\n\0";
//
//// Function to generate a sphere
//void generateSphere(float radius, int sectors, int stacks, std::vector<float>& vertices, std::vector<unsigned int>& indices) {
//    float x, y, z, xy;
//    float sectorStep = 2 * M_PI / sectors;
//    float stackStep = M_PI / stacks;
//    float sectorAngle, stackAngle;
//
//    for (int i = 0; i <= stacks; ++i) {
//        stackAngle = M_PI / 2 - i * stackStep;
//        xy = radius * cosf(stackAngle);
//        z = radius * sinf(stackAngle);
//
//        for (int j = 0; j <= sectors; ++j) {
//            sectorAngle = j * sectorStep;
//            x = xy * cosf(sectorAngle);
//            y = xy * sinf(sectorAngle);
//            vertices.push_back(x);
//            vertices.push_back(y);
//            vertices.push_back(z);
//            vertices.push_back(x / radius);
//            vertices.push_back(y / radius);
//            vertices.push_back(z / radius);
//            vertices.push_back((float)j / sectors);
//            vertices.push_back((float)i / stacks);
//        }
//    }
//
//    int k1, k2;
//    for (int i = 0; i < stacks; ++i) {
//        k1 = i * (sectors + 1);
//        k2 = k1 + sectors + 1;
//
//        for (int j = 0; j < sectors; ++j, ++k1, ++k2) {
//            if (i != 0) {
//                indices.push_back(k1);
//                indices.push_back(k2);
//                indices.push_back(k1 + 1);
//            }
//
//            if (i != (stacks - 1)) {
//                indices.push_back(k1 + 1);
//                indices.push_back(k2);
//                indices.push_back(k2 + 1);
//            }
//        }
//    }
//}
//
//// Function to generate a torus
//void generateTorus(float majorRadius, float minorRadius, int majorSegments, int minorSegments, std::vector<float>& vertices, std::vector<unsigned int>& indices) {
//    for (int i = 0; i <= majorSegments; ++i) {
//        float majorAngle = 2.0f * M_PI * i / majorSegments;
//        glm::vec3 majorCircle(majorRadius * cosf(majorAngle), 0.0f, majorRadius * sinf(majorAngle));
//
//        for (int j = 0; j <= minorSegments; ++j) {
//            float minorAngle = 2.0f * M_PI * j / minorSegments;
//            glm::vec3 minorCircle(minorRadius * cosf(minorAngle), minorRadius * sinf(minorAngle), 0.0f);
//
//            glm::vec3 vertex = majorCircle + glm::vec3(cosf(majorAngle) * minorCircle.x, minorCircle.y, sinf(majorAngle) * minorCircle.x);
//            glm::vec3 normal = glm::normalize(vertex - majorCircle);
//
//            vertices.push_back(vertex.x);
//            vertices.push_back(vertex.y);
//            vertices.push_back(vertex.z);
//            vertices.push_back(normal.x);
//            vertices.push_back(normal.y);
//            vertices.push_back(normal.z);
//            vertices.push_back((float)i / majorSegments);
//            vertices.push_back((float)j / minorSegments);
//        }
//    }
//
//    for (int i = 0; i < majorSegments; ++i) {
//        for (int j = 0; j < minorSegments; ++j) {
//            int first = (i * (minorSegments + 1)) + j;
//            int second = first + minorSegments + 1;
//
//            indices.push_back(first);
//            indices.push_back(second);
//            indices.push_back(first + 1);
//
//            indices.push_back(second);
//            indices.push_back(second + 1);
//            indices.push_back(first + 1);
//        }
//    }
//}
//
//void generatePyramid(std::vector<float>& vertices, std::vector<unsigned int>& indices) {
//    // Base vertices
//    vertices = {
//        // Positions          // Normals           // Texture Coords
//        // Base
//        -0.5f, 0.0f, -0.5f,  0.0f, -1.0f, 0.0f,  0.0f, 0.0f,
//         0.5f, 0.0f, -0.5f,  0.0f, -1.0f, 0.0f,  1.0f, 0.0f,
//         0.5f, 0.0f,  0.5f,  0.0f, -1.0f, 0.0f,  1.0f, 1.0f,
//        -0.5f, 0.0f,  0.5f,  0.0f, -1.0f, 0.0f,  0.0f, 1.0f,
//
//        // Front face
//         0.0f, 1.0f,  0.0f,  0.0f,  0.707f,  0.707f,  0.5f, 0.5f,
//        -0.5f, 0.0f,  0.5f,  0.0f,  0.707f,  0.707f,  0.0f, 1.0f,
//         0.5f, 0.0f,  0.5f,  0.0f,  0.707f,  0.707f,  1.0f, 1.0f,
//
//         // Right face
//          0.0f, 1.0f,  0.0f,  0.707f,  0.707f,  0.0f,  0.5f, 0.5f,
//          0.5f, 0.0f,  0.5f,  0.707f,  0.707f,  0.0f,  1.0f, 1.0f,
//          0.5f, 0.0f, -0.5f,  0.707f,  0.707f,  0.0f,  1.0f, 0.0f,
//
//          // Back face
//           0.0f, 1.0f,  0.0f,  0.0f,  0.707f, -0.707f,  0.5f, 0.5f,
//           0.5f, 0.0f, -0.5f,  0.0f,  0.707f, -0.707f,  1.0f, 0.0f,
//          -0.5f, 0.0f, -0.5f,  0.0f,  0.707f, -0.707f,  0.0f, 0.0f,
//
//          // Left face
//           0.0f, 1.0f,  0.0f, -0.707f,  0.707f,  0.0f,  0.5f, 0.5f,
//          -0.5f, 0.0f, -0.5f, -0.707f,  0.707f,  0.0f,  0.0f, 0.0f,
//          -0.5f, 0.0f,  0.5f, -0.707f,  0.707f,  0.0f,  0.0f, 1.0f
//    };
//
//    // Indices for the base and sides
//    indices = {
//        // Base
//        0, 1, 2,
//        2, 3, 0,
//
//        // Front face
//        4, 5, 6,
//
//        // Right face
//        7, 8, 9,
//
//        // Back face
//        10, 11, 12,
//
//        // Left face
//        13, 14, 15
//    };
//}
//
//void CreateCube(std::vector<float>& vertices, std::vector<unsigned int>& indices) {
//    // Wierzcho³ki szeœcianu
//    vertices = {
//        // Positions          // Normals           // Texture Coords
//        // Back face
//        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
//         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
//         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
//        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
//
//        // Front face
//        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
//         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
//         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
//        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
//
//        // Left face
//        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
//        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
//        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
//        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
//
//        // Right face
//         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
//         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
//         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
//         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
//
//         // Bottom face
//         -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
//          0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
//          0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
//         -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
//
//         // Top face
//         -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
//          0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
//          0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
//         -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
//    };
//
//    // Indeksy dla szeœcianu
//    indices = {
//        // Back face
//        0, 1, 2,
//        2, 3, 0,
//
//        // Front face
//        4, 5, 6,
//        6, 7, 4,
//
//        // Left face
//        8, 9, 10,
//        10, 11, 8,
//
//        // Right face
//        12, 13, 14,
//        14, 15, 12,
//
//        // Bottom face
//        16, 17, 18,
//        18, 19, 16,
//
//        // Top face
//        20, 21, 22,
//        22, 23, 20
//    };
//}
//
//int main()
//{
//    // glfw: initialize and configure
//    glfwInit();
//    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
//    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
//    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
//
//#ifdef __APPLE__
//    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
//#endif
//
//    // glfw window creation
//    GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearnOpenGL", NULL, NULL);
//    if (window == NULL)
//    {
//        std::cout << "Failed to create GLFW window" << std::endl;
//        glfwTerminate();
//        return -1;
//    }
//    glfwMakeContextCurrent(window);
//    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
//    glfwSetCursorPosCallback(window, mouse_callback);
//    glfwSetScrollCallback(window, scroll_callback);
//
//    // Capture the mouse
//    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
//
//    // glad: load all OpenGL function pointers
//    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
//    {
//        std::cout << "Failed to initialize GLAD" << std::endl;
//        return -1;
//    }
//
//    // build and compile our shader program
//    unsigned int vertexShader = glCreateShader(GL_VERTEX_SHADER);
//    glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
//    glCompileShader(vertexShader);
//    int success;
//    char infoLog[512];
//    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
//    if (!success)
//    {
//        glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
//        std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//    }
//    unsigned int fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
//    glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
//    glCompileShader(fragmentShader);
//    glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
//    if (!success)
//    {
//        glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
//        std::cout << "ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n" << infoLog << std::endl;
//    }
//    unsigned int shaderProgram = glCreateProgram();
//    glAttachShader(shaderProgram, vertexShader);
//    glAttachShader(shaderProgram, fragmentShader);
//    glLinkProgram(shaderProgram);
//    glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
//    if (!success) {
//        glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
//        std::cout << "ERROR::SHADER::PROGRAM::LINKING_FAILED\n" << infoLog << std::endl;
//    }
//    glDeleteShader(vertexShader);
//    glDeleteShader(fragmentShader);
//
//    // Generate sphere
//    std::vector<float> sphereVertices;
//    std::vector<unsigned int> sphereIndices;
//    generateSphere(0.2f, 36, 18, sphereVertices, sphereIndices); // Ma³a kula jako Ÿród³o œwiat³a
//
//    // Sphere VAO, VBO, EBO
//    unsigned int sphereVAO, sphereVBO, sphereEBO;
//    glGenVertexArrays(1, &sphereVAO);
//    glGenBuffers(1, &sphereVBO);
//    glGenBuffers(1, &sphereEBO);
//    glBindVertexArray(sphereVAO);
//    glBindBuffer(GL_ARRAY_BUFFER, sphereVBO);
//    glBufferData(GL_ARRAY_BUFFER, sphereVertices.size() * sizeof(float), sphereVertices.data(), GL_STATIC_DRAW);
//    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, sphereEBO);
//    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sphereIndices.size() * sizeof(unsigned int), sphereIndices.data(), GL_STATIC_DRAW);
//    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
//    glEnableVertexAttribArray(0);
//    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
//    glEnableVertexAttribArray(1);
//    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
//    glEnableVertexAttribArray(2);
//
//    // Generate Cube
//    std::vector<float> cubeVertices;
//    std::vector<unsigned int> cubeIndices;
//    CreateCube(cubeVertices, cubeIndices);
//
//    unsigned int cubeVAO, cubeVBO, cubeEBO;
//    glGenVertexArrays(1, &cubeVAO);
//    glGenBuffers(1, &cubeVBO);
//    glGenBuffers(1, &cubeEBO);
//
//    glBindVertexArray(cubeVAO);
//
//    glBindBuffer(GL_ARRAY_BUFFER, cubeVBO);
//    glBufferData(GL_ARRAY_BUFFER, cubeVertices.size() * sizeof(float), cubeVertices.data(), GL_STATIC_DRAW);
//
//    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, cubeEBO);
//    glBufferData(GL_ELEMENT_ARRAY_BUFFER, cubeIndices.size() * sizeof(unsigned int), cubeIndices.data(), GL_STATIC_DRAW);
//
//    // Ustaw atrybuty wierzcho³ków
//    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
//    glEnableVertexAttribArray(0);
//
//    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
//    glEnableVertexAttribArray(1);
//
//    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
//    glEnableVertexAttribArray(2);
//
//    glBindVertexArray(0);
//
//    // Generate torus
//    std::vector<float> torusVertices;
//    std::vector<unsigned int> torusIndices;
//    generateTorus(1.0f, 0.3f, 36, 18, torusVertices, torusIndices);
//
//    // Torus VAO, VBO, EBO
//    unsigned int torusVAO, torusVBO, torusEBO;
//    glGenVertexArrays(1, &torusVAO);
//    glGenBuffers(1, &torusVBO);
//    glGenBuffers(1, &torusEBO);
//    glBindVertexArray(torusVAO);
//    glBindBuffer(GL_ARRAY_BUFFER, torusVBO);
//    glBufferData(GL_ARRAY_BUFFER, torusVertices.size() * sizeof(float), torusVertices.data(), GL_STATIC_DRAW);
//    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, torusEBO);
//    glBufferData(GL_ELEMENT_ARRAY_BUFFER, torusIndices.size() * sizeof(unsigned int), torusIndices.data(), GL_STATIC_DRAW);
//    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
//    glEnableVertexAttribArray(0);
//    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
//    glEnableVertexAttribArray(1);
//    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
//    glEnableVertexAttribArray(2);
//
//    // Generate pyramid
//    std::vector<float> pyramidVertices;
//    std::vector<unsigned int> pyramidIndices;
//    generatePyramid(pyramidVertices, pyramidIndices);
//
//    // Pyramid VAO, VBO, EBO
//    unsigned int pyramidVAO, pyramidVBO, pyramidEBO;
//    glGenVertexArrays(1, &pyramidVAO);
//    glGenBuffers(1, &pyramidVBO);
//    glGenBuffers(1, &pyramidEBO);
//    glBindVertexArray(pyramidVAO);
//    glBindBuffer(GL_ARRAY_BUFFER, pyramidVBO);
//    glBufferData(GL_ARRAY_BUFFER, pyramidVertices.size() * sizeof(float), pyramidVertices.data(), GL_STATIC_DRAW);
//    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, pyramidEBO);
//    glBufferData(GL_ELEMENT_ARRAY_BUFFER, pyramidIndices.size() * sizeof(unsigned int), pyramidIndices.data(), GL_STATIC_DRAW);
//    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
//    glEnableVertexAttribArray(0);
//    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
//    glEnableVertexAttribArray(1);
//    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
//    glEnableVertexAttribArray(2);
//
//    // Floor VAO, VBO
//    float floorVertices[] = {
//        // Positions          // Normals           // Texture Coords
//        -15.0f, 0.0f, -30.0f,  0.0f, 1.0f, 0.0f,  0.0f, 0.0f, // Zwiêkszona pod³oga
//         15.0f, 0.0f, -30.0f,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f,
//         15.0f, 0.0f,  5.0f,  0.0f, 1.0f, 0.0f,  1.0f, 1.0f,
//        -15.0f, 0.0f,  5.0f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f
//    };
//    unsigned int floorIndices[] = {
//        0, 1, 2,
//        2, 3, 0
//    };
//
//    // Calculate tangent and bitangent for the floor
//    glm::vec3 pos1(floorVertices[0], floorVertices[1], floorVertices[2]);
//    glm::vec3 pos2(floorVertices[8], floorVertices[9], floorVertices[10]);
//    glm::vec3 pos3(floorVertices[16], floorVertices[17], floorVertices[18]);
//
//    glm::vec2 uv1(floorVertices[6], floorVertices[7]);
//    glm::vec2 uv2(floorVertices[14], floorVertices[15]);
//    glm::vec2 uv3(floorVertices[22], floorVertices[23]);
//
//    glm::vec3 edge1 = pos2 - pos1;
//    glm::vec3 edge2 = pos3 - pos1;
//    glm::vec2 deltaUV1 = uv2 - uv1;
//    glm::vec2 deltaUV2 = uv3 - uv1;
//
//    float f = 1.0f / (deltaUV1.x * deltaUV2.y - deltaUV2.x * deltaUV1.y);
//
//    glm::vec3 tangent;
//    tangent.x = f * (deltaUV2.y * edge1.x - deltaUV1.y * edge2.x);
//    tangent.y = f * (deltaUV2.y * edge1.y - deltaUV1.y * edge2.y);
//    tangent.z = f * (deltaUV2.y * edge1.z - deltaUV1.y * edge2.z);
//    tangent = glm::normalize(tangent);
//
//    glm::vec3 bitangent;
//    bitangent.x = f * (-deltaUV2.x * edge1.x + deltaUV1.x * edge2.x);
//    bitangent.y = f * (-deltaUV2.x * edge1.y + deltaUV1.x * edge2.y);
//    bitangent.z = f * (-deltaUV2.x * edge1.z + deltaUV1.x * edge2.z);
//    bitangent = glm::normalize(bitangent);
//
//    // Add tangent and bitangent to floor vertices
//    float floorVerticesWithTangents[] = {
//        // Positions          // Normals           // Texture Coords  // Tangent       // Bitangent
//        -15.0f, 0.0f, -30.0f,  0.0f, 1.0f, 0.0f,  0.0f, 0.0f,  tangent.x, tangent.y, tangent.z,  bitangent.x, bitangent.y, bitangent.z,
//         15.0f, 0.0f, -30.0f,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f,  tangent.x, tangent.y, tangent.z,  bitangent.x, bitangent.y, bitangent.z,
//         15.0f, 0.0f,  5.0f,  0.0f, 1.0f, 0.0f,  1.0f, 1.0f,  tangent.x, tangent.y, tangent.z,  bitangent.x, bitangent.y, bitangent.z,
//        -15.0f, 0.0f,  5.0f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f,  tangent.x, tangent.y, tangent.z,  bitangent.x, bitangent.y, bitangent.z
//    };
//
//    unsigned int floorVAO, floorVBO, floorEBO;
//    glGenVertexArrays(1, &floorVAO);
//    glGenBuffers(1, &floorVBO);
//    glGenBuffers(1, &floorEBO);
//    glBindVertexArray(floorVAO);
//    glBindBuffer(GL_ARRAY_BUFFER, floorVBO);
//    glBufferData(GL_ARRAY_BUFFER, sizeof(floorVerticesWithTangents), floorVerticesWithTangents, GL_STATIC_DRAW);
//    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, floorEBO);
//    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(floorIndices), floorIndices, GL_STATIC_DRAW);
//    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 14 * sizeof(float), (void*)0);
//    glEnableVertexAttribArray(0);
//    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 14 * sizeof(float), (void*)(3 * sizeof(float)));
//    glEnableVertexAttribArray(1);
//    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 14 * sizeof(float), (void*)(6 * sizeof(float)));
//    glEnableVertexAttribArray(2);
//    glVertexAttribPointer(3, 3, GL_FLOAT, GL_FALSE, 14 * sizeof(float), (void*)(8 * sizeof(float)));
//    glEnableVertexAttribArray(3);
//    glVertexAttribPointer(4, 3, GL_FLOAT, GL_FALSE, 14 * sizeof(float), (void*)(11 * sizeof(float)));
//    glEnableVertexAttribArray(4);
//
//    // Load texture for the floor
//    unsigned int floorTexture = loadTexture("grass_texture.jpg");
//    unsigned int objectTexture = loadTexture("wood_texture.jpg");
//    unsigned int normalMap = loadTexture("normal_map2.png");
//
//    // Unbind VAO
//    glBindVertexArray(0);
//
//    // Enable depth testing
//    glEnable(GL_DEPTH_TEST);
//    spotlightDir = glm::vec3(0.0f, -0.2f, -1.0f); // Reflektor skierowany do przodu i lekko w dó³
//    glClearColor(0.53f, 0.81f, 0.92f, 1.0f);
//    while (!glfwWindowShouldClose(window))
//    {
//        // Per-frame time logic
//        float currentFrame = glfwGetTime();
//        deltaTime = currentFrame - lastFrame;
//        lastFrame = currentFrame;
//
//        // Input
//        processInput(window);
//
//        // Clear the color and depth buffer
//        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
//
//        // Use shader program
//        glUseProgram(shaderProgram);
//
//        // Pass fog and day/night settings to the shader
//        glUniform1f(glGetUniformLocation(shaderProgram, "fogDensity"), fogDensity);
//        glUniform1i(glGetUniformLocation(shaderProgram, "isDay"), isDay);
//
//        // Pass new static light properties to the shader
//        glUniform3f(glGetUniformLocation(shaderProgram, "staticPyramidLightPos"), staticPyramidLightPos.x, staticPyramidLightPos.y, staticPyramidLightPos.z);
//        glUniform3f(glGetUniformLocation(shaderProgram, "staticPyramidLightColor"), staticPyramidLightColor.x, staticPyramidLightColor.y, staticPyramidLightColor.z);
//
//        // Create transformations
//        glm::mat4 view;
//        if (currentCameraMode == STATIC) {
//            view = glm::lookAt(cameraPos, cameraPos + cameraFront, cameraUp);
//        }
//        else if (currentCameraMode == FOLLOW_CAR) {
//            view = glm::lookAt(glm::vec3(carPosX, 2.0f, 5.0f), glm::vec3(carPosX, 0.0f, 0.0f), cameraUp);
//        }
//        else if (currentCameraMode == CAR_FIRST_PERSON) {
//            // Kamera nr 3: obraca siê, aby œledziæ poruszaj¹cy siê obiekt
//            glm::vec3 carPos = glm::vec3(carPosX, 0.5f, -2.0f);
//            glm::vec3 cameraTarget = carPos;
//            glm::vec3 cameraDirection = glm::normalize(cameraTarget - cameraPos);
//            view = glm::lookAt(cameraPos, cameraPos + cameraDirection, cameraUp);
//        }
//        glm::mat4 projection = glm::perspective(glm::radians(45.0f), (float)SCR_WIDTH / (float)SCR_HEIGHT, 0.1f, 100.0f);
//
//        // Pass transformation matrices to the shader
//        unsigned int viewLoc = glGetUniformLocation(shaderProgram, "view");
//        glUniformMatrix4fv(viewLoc, 1, GL_FALSE, &view[0][0]);
//        unsigned int projectionLoc = glGetUniformLocation(shaderProgram, "projection");
//        glUniformMatrix4fv(projectionLoc, 1, GL_FALSE, &projection[0][0]);
//
//        // Draw floor
//        glBindTexture(GL_TEXTURE_2D, floorTexture);
//        glBindTexture(GL_TEXTURE_2D, normalMap); // Bind normal map for the floor
//        glm::mat4 model = glm::mat4(1.0f);
//        unsigned int modelLoc = glGetUniformLocation(shaderProgram, "model");
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindVertexArray(floorVAO);
//        glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
//
//
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        // Draw sphere (light source)
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, pointLightPos);
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindVertexArray(sphereVAO);
//        glDrawElements(GL_TRIANGLES, sphereIndices.size(), GL_UNSIGNED_INT, 0);
//
//        // Draw rotating tower
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(3.0f, 3.0f, -6.0f));
//        model = glm::rotate(model, (float)glfwGetTime(), glm::vec3(0.0f, 1.0f, 0.0f)); // Obrót tylko wokó³ osi Y
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(pyramidVAO);
//        glDrawElements(GL_TRIANGLES, pyramidIndices.size(), GL_UNSIGNED_INT, 0);
//
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(3.0f, 1.5f, -6.0f));
//        model = glm::rotate(model, (float)glfwGetTime(), glm::vec3(0.0f, 1.0f, 0.0f)); // Obrót tylko wokó³ osi Y
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(3.0f, 0.5f, -6.0f));
//        model = glm::rotate(model, (float)glfwGetTime(), glm::vec3(0.0f, 1.0f, 0.0f)); // Obrót tylko wokó³ osi Y
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(3.0f, 2.5f, -6.0f));
//        model = glm::rotate(model, (float)glfwGetTime(), glm::vec3(0.0f, 1.0f, 0.0f)); // Obrót tylko wokó³ osi Y
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        // Draw car (simple cube as a placeholder)
//        model = glm::mat4(1.0f);
//        carPosX += carSpeed * deltaTime;
//        if (carPosX > 3.0f || carPosX < -3.0f) carSpeed *= -1;
//        model = glm::translate(model, glm::vec3(carPosX, 0.5f, -2.0f)); // Inna wspó³rzêdna Z
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        // Draw torus
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(-2.0f, 0.5f, 0.0f));
//        model = glm::rotate(model, (float)glfwGetTime() * 0.5f, glm::vec3(0.0f, 1.0f, 0.0f));
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(torusVAO);
//        glDrawElements(GL_TRIANGLES, torusIndices.size(), GL_UNSIGNED_INT, 0);
//
//        // Draw sphere (light source)
//        model = glm::mat4(3.0f);
//        model = glm::translate(model, glm::vec3(1.5f, 0.2f, -1.0f));
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindVertexArray(sphereVAO);
//        glDrawElements(GL_TRIANGLES, sphereIndices.size(), GL_UNSIGNED_INT, 0);
//
//        // Draw additional objects in the distance
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(-5.0f, 0.5f, -8.0f));
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(-3.0f, 0.5f, -12.0f));
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(-1.0f, 0.5f, -15.0f));
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        model = glm::mat4(1.0f);
//        model = glm::translate(model, glm::vec3(1.0f, 0.5f, -22.0f));
//        glUniformMatrix4fv(modelLoc, 1, GL_FALSE, &model[0][0]);
//        glBindTexture(GL_TEXTURE_2D, objectTexture);
//        glBindVertexArray(cubeVAO);
//        glDrawElements(GL_TRIANGLES, 36, GL_UNSIGNED_INT, 0);
//
//        // Update spotlight position and direction
//        pointLightPos = glm::vec3(carPosX, 1.0f, -2.0f); // Reflektor pod¹¿a za samochodem
//
//        // Pass light properties to the shader
//        glUniform3f(glGetUniformLocation(shaderProgram, "pointLightPos"), pointLightPos.x, pointLightPos.y, pointLightPos.z);
//        glUniform3f(glGetUniformLocation(shaderProgram, "pointLightColor"), pointLightColor.x, pointLightColor.y, pointLightColor.z);
//        glUniform3f(glGetUniformLocation(shaderProgram, "spotlightDir"), spotlightDir.x, spotlightDir.y, spotlightDir.z);
//        glUniform3f(glGetUniformLocation(shaderProgram, "spotlightColor"), 1.0f, 1.0f, 1.0f); // Bia³y reflektor
//        glUniform1f(glGetUniformLocation(shaderProgram, "spotlightCutOff"), spotlightCutOff);
//
//        // Pass view position to the shader
//        glUniform3f(glGetUniformLocation(shaderProgram, "viewPos"), cameraPos.x, cameraPos.y, cameraPos.z);
//
//        // Set object color (e.g., for the sphere and cube)
//        glUniform3f(glGetUniformLocation(shaderProgram, "objectColor"), 1.0f, 0.5f, 0.2f); // Pomarañczowy kolor
//
//        // Swap buffers and poll IO events
//        glfwSwapBuffers(window);
//        glfwPollEvents();
//    }
//
//    // De-allocate resources
//    glDeleteVertexArrays(1, &sphereVAO);
//    glDeleteBuffers(1, &sphereVBO);
//    glDeleteBuffers(1, &sphereEBO);
//    glDeleteVertexArrays(1, &cubeVAO);
//    glDeleteBuffers(1, &cubeVBO);
//    glDeleteBuffers(1, &cubeEBO);
//    glDeleteVertexArrays(1, &torusVAO);
//    glDeleteBuffers(1, &torusVBO);
//    glDeleteBuffers(1, &torusEBO);
//    glDeleteVertexArrays(1, &floorVAO);
//    glDeleteBuffers(1, &floorVBO);
//    glDeleteBuffers(1, &floorEBO);
//    glDeleteProgram(shaderProgram);
//
//    // Terminate GLFW
//    glfwTerminate();
//    return 0;
//}
//
//// Process all input: query GLFW whether relevant keys are pressed/released this frame and react accordingly
//void processInput(GLFWwindow* window)
//{
//    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
//        glfwSetWindowShouldClose(window, true);
//
//    float cameraSpeed = 2.5f * deltaTime;
//    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
//        cameraPos += cameraSpeed * cameraFront;
//    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
//        cameraPos -= cameraSpeed * cameraFront;
//    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
//        cameraPos -= glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
//    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
//        cameraPos += glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
//
//    // Change camera mode
//    if (glfwGetKey(window, GLFW_KEY_1) == GLFW_PRESS)
//        currentCameraMode = STATIC;
//    if (glfwGetKey(window, GLFW_KEY_2) == GLFW_PRESS)
//        currentCameraMode = FOLLOW_CAR;
//    if (glfwGetKey(window, GLFW_KEY_3) == GLFW_PRESS)
//        currentCameraMode = CAR_FIRST_PERSON;
//
//    // Toggle day/night
//    if (glfwGetKey(window, GLFW_KEY_N) == GLFW_PRESS) {
//        isDay = !isDay;
//        if (isDay) {
//            pointLightColor = glm::vec3(0.8f, 0.8f, 0.8f); // Bia³e œwiat³o w dzieñ
//            glClearColor(0.53f, 0.81f, 0.92f, 1.0f);
//        }
//        else {
//            pointLightColor = glm::vec3(0.1f, 0.1f, 0.1f); // Ciemniejsze œwiat³o w nocy
//            glClearColor(0.0f, 0.0f, 0.2f, 1.0f); // Ciemno granatowy kolor (R=0.0, G=0.0, B=0.2, Alpha=1.0)
//        }
//    }
//
//    // Increase fog density
//    if (glfwGetKey(window, GLFW_KEY_K) == GLFW_PRESS) {
//        fogDensity += 0.0001f;
//        if (fogDensity > 0.3f) fogDensity = 0.3f;
//    }
//
//    // Decrease fog density
//    if (glfwGetKey(window, GLFW_KEY_L) == GLFW_PRESS) {
//        fogDensity -= 0.0001f;
//        if (fogDensity < 0.01f) fogDensity = 0.01f;
//    }
//
//    // Change spotlight direction with arrow keys
//    if (glfwGetKey(window, GLFW_KEY_UP) == GLFW_PRESS) {
//        spotlightDir.y += 0.001f;
//    }
//    if (glfwGetKey(window, GLFW_KEY_DOWN) == GLFW_PRESS) {
//        spotlightDir.y -= 0.001f;
//    }
//    if (glfwGetKey(window, GLFW_KEY_LEFT) == GLFW_PRESS) {
//        spotlightDir.x -= 0.001f;
//    }
//    if (glfwGetKey(window, GLFW_KEY_RIGHT) == GLFW_PRESS) {
//        spotlightDir.x += 0.001f;
//    }
//
//    // Normalize the spotlight direction to ensure consistent behavior
//    spotlightDir = glm::normalize(spotlightDir);
//}
//
//// glfw: whenever the window size changed (by OS or user resize) this callback function executes
//void framebuffer_size_callback(GLFWwindow* window, int width, int height)
//{
//    glViewport(0, 0, width, height);
//}
//
//// glfw: whenever the mouse moves, this callback is called
//void mouse_callback(GLFWwindow* window, double xpos, double ypos)
//{
//    if (firstMouse)
//    {
//        lastX = xpos;
//        lastY = ypos;
//        firstMouse = false;
//    }
//
//    float xoffset = xpos - lastX;
//    float yoffset = lastY - ypos; // reversed since y-coordinates go from bottom to top
//    lastX = xpos;
//    lastY = ypos;
//
//    float sensitivity = 0.1f;
//    xoffset *= sensitivity;
//    yoffset *= sensitivity;
//
//    yaw += xoffset;
//    pitch += yoffset;
//
//    // Make sure that when pitch is out of bounds, screen doesn't get flipped
//    if (pitch > 89.0f)
//        pitch = 89.0f;
//    if (pitch < -89.0f)
//        pitch = -89.0f;
//
//    glm::vec3 front;
//    front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
//    front.y = sin(glm::radians(pitch));
//    front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
//    cameraFront = glm::normalize(front);
//}
//
//// glfw: whenever the mouse scroll wheel scrolls, this callback is called
//void scroll_callback(GLFWwindow* window, double xoffset, double yoffset)
//{
//    // Adjust FOV based on scroll input
//    float fov = glm::radians(45.0f) - (float)yoffset;
//    if (fov < glm::radians(1.0f))
//        fov = glm::radians(1.0f);
//    if (fov > glm::radians(45.0f))
//        fov = glm::radians(45.0f);
//}
//
//// Utility function to load a texture
//unsigned int loadTexture(const char* path)
//{
//    unsigned int textureID;
//    glGenTextures(1, &textureID);
//
//    int width, height, nrComponents;
//    unsigned char* data = stbi_load(path, &width, &height, &nrComponents, 0);
//    if (data)
//    {
//        GLenum format;
//        if (nrComponents == 1)
//            format = GL_RED;
//        else if (nrComponents == 3)
//            format = GL_RGB;
//        else if (nrComponents == 4)
//            format = GL_RGBA;
//
//        glBindTexture(GL_TEXTURE_2D, textureID);
//        glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
//        glGenerateMipmap(GL_TEXTURE_2D);
//
//        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
//        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
//        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
//        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
//
//        stbi_image_free(data);
//    }
//    else
//    {
//        std::cout << "Texture failed to load at path: " << path << std::endl;
//        stbi_image_free(data);
//    }
//
//    return textureID;
//}