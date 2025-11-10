# Microsoft Agent Framework Demo Project 🌟

Welcome to the **Microsoft Agent Framework Demo Project**! This repository contains a collection of 10 comprehensive demos showcasing the powerful capabilities of Microsoft's newly released Agent Framework. Each demo is designed to highlight specific features and use cases, providing a hands-on experience for developers exploring the world of AI agents. 🚀

---

## Table of Contents 📚

1. [Project Overview](#project-overview)
2. [Demos](#demos)
   - [Demo 1: Joker Agent 🃏](#demo-1-joker-agent-)
   - [Demo 2: Light Plugin 💡](#demo-2-light-plugin-)
   - [Demo 3: Human in the Loop 👥](#demo-3-human-in-the-loop-)
   - [Demo 4: MCP GitHub Integration 🔗](#demo-4-mcp-github-integration-)
   - [Demo 5: Agent Observability 📊](#demo-5-agent-observability-)
   - [Demo 6: Sequential Workflows ⚡](#demo-6-sequential-workflows-)
   - [Demo 7: Concurrent Workflows 🔄](#demo-7-concurrent-workflows-)
   - [Demo 8: Workflow Handoffs 🤝](#demo-8-workflow-handoffs-)
   - [Demo 9: Development UI (Python) 🐍](#demo-9-development-ui-python-)
   - [Demo 10: DevUI Workflow Demo (Python) 🔀](#demo-10-devui-workflow-demo-python-)
3. [Setup Instructions](#setup-instructions)
4. [Running with .NET Aspire](#running-with-net-aspire)
5. [Contributing](#contributing)
6. [License](#license)

---

## Project Overview 📝

The Microsoft Agent Framework Demo Project showcases the powerful capabilities of Microsoft's newly released Agent Framework through 10 comprehensive demos. Each demo explores different aspects of building intelligent agents, from basic conversational AI to complex multi-agent workflows and integrations.

🎯 **What you'll learn:**
- Creating and configuring AI agents with custom instructions
- Building agent tools and plugins for specialized functionality  
- Implementing human-in-the-loop patterns for interactive experiences
- Integrating with external systems using Model Context Protocol (MCP)
- Adding observability and telemetry to monitor agent performance
- Orchestrating multiple agents in sequential and concurrent workflows
- Designing sophisticated agent handoff patterns

Whether you're a beginner exploring AI agents or an experienced developer looking to leverage Microsoft's Agent Framework, these demos provide practical, hands-on examples to accelerate your journey! 🚀

---

## Demos 🎥

### Demo 1: Joker Agent 🃏

**What you'll learn:**
- Creating your first AI agent with Microsoft Agent Framework
- Configuring agent instructions and personality
- Implementing basic chat functionality

**Description:**
Jump into the world of AI agents with this fun and friendly introduction! The Joker Agent demonstrates the fundamentals of creating an AI agent that specializes in telling jokes. Perfect for understanding the core concepts of agent creation, instruction setting, and basic conversational interactions.

**Key Features:**
- ✨ Simple agent creation with custom instructions
- 🎭 Personality-driven responses 
- 💬 Interactive chat interface
- 🚀 Perfect starting point for beginners

**Setup:**
- Navigate to `src/Demo001_JokerAgentDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Run `dotnet run` or press F5 in Visual Studio

**Try these prompts:**
- "Tell me a joke about a pirate"
- "Tell me a joke about AI"
- "Tell me a joke about Portugal"

---

### Demo 2: Light Plugin 💡

**What you'll learn:**
- Building agent tools and plugins
- Integrating external functionality into agents
- Understanding function calling capabilities

**Description:**
Discover the power of agent tools! This demo shows how to create custom plugins that extend your agent's capabilities. The Light Plugin allows your agent to control and query the state of various lights, demonstrating how agents can interact with external systems and APIs.

**Key Features:**
- 🔧 Custom plugin development
- ⚡ Function calling with structured responses
- 🎮 Interactive light control system
- 📊 State management and queries

**Setup:**
- Navigate to `src/Demo002_LightPluginDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Run `dotnet run` or press F5 in Visual Studio

**Try these commands:**
- "Is the Entrance light on?"
- "Turn on the Outside light"
- "Change the color of the Main Stage light to blue"
- "Set the brightness of the Second Stage light to low"

---

### Demo 3: Human in the Loop 👥

**What you'll learn:**
- Implementing human approval workflows
- Managing agent-human interactions
- Building collaborative AI experiences

**Description:**
Sometimes agents need human guidance! This demo extends the Light Plugin with human-in-the-loop functionality, requiring user confirmation before executing certain actions. Learn how to build collaborative experiences where humans and agents work together effectively.

**Key Features:**
- 🤝 Human approval workflows
- ✅ Confirmation prompts for critical actions
- 🔒 Safe agent operation patterns
- 👥 Collaborative decision making

**Setup:**
- Navigate to `src/Demo003_HumanInTheLoopDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Run `dotnet run` or press F5 in Visual Studio

**Try these interactions:**
- Request light changes and approve/deny the actions
- Observe how the agent asks for permission before executing
- Experience collaborative agent workflows

---

### Demo 4: MCP GitHub Integration 🔗

**What you'll learn:**
- Integrating with external systems using Model Context Protocol (MCP)
- Connecting agents to GitHub repositories
- Working with containerized MCP servers

**Description:**
Expand your agent's reach! This demo demonstrates how to connect your agent to GitHub using the Model Context Protocol (MCP). Your agent can query repositories, analyze commits, and interact with GitHub data, showcasing the power of external system integration.

**Key Features:**
- 🔗 Model Context Protocol integration
- 🐙 GitHub repository access
- 🐳 Docker/Podman containerized server
- 📊 Repository analysis capabilities

**Setup:**
- Navigate to `src/Demo004_MCPGitHubDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Ensure Docker or Podman is installed and running
- Run `dotnet run` or press F5 in Visual Studio

**Try these queries:**
- "Summarize the last four commits to the microsoft/agent-framework repository"
- "What are the latest issues in the repository?"
- "Tell me about the repository structure"

---

### Demo 5: Agent Observability 📊

**What you'll learn:**
- Adding telemetry and monitoring to agents
- Implementing OpenTelemetry for agent tracing
- Debugging and performance monitoring

**Description:**
Make your agents observable! This demo shows how to add comprehensive telemetry and monitoring to your AI agents using OpenTelemetry. Monitor agent performance, trace conversations, and gain insights into your agent's behavior and performance.

**Key Features:**
- 📈 OpenTelemetry integration
- 🔍 Conversation tracing
- 📊 Performance monitoring
- 🐛 Debugging capabilities

**Setup:**
- Navigate to `src/Demo005_AgentObservabilityDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Run `dotnet run` or press F5 in Visual Studio
- Observe telemetry output in the console

**Observe:**
- Detailed tracing information for each agent interaction
- Performance metrics and timing data
- Debug information for troubleshooting

---

### Demo 6: Sequential Workflows ⚡

**What you'll learn:**
- Building sequential agent workflows
- Chaining multiple agents together
- Creating translation pipelines

**Description:**
Chain agents for powerful workflows! This demo creates a sequential translation pipeline where text flows through multiple translation agents one after another. Perfect for understanding how to orchestrate multiple agents in a coordinated sequence.

**Key Features:**
- 🔗 Sequential agent chaining
- 🌍 Multi-language translation pipeline
- ⚡ Streaming workflow execution
- 📋 Step-by-step processing

**Setup:**
- Navigate to `src/Demo006_WorkflowSequentialDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Run `dotnet run` or press F5 in Visual Studio

**Watch as your text gets translated:**
1. Portuguese → French → Spanish in sequence
2. Each agent builds upon the previous agent's output
3. Stream results to see the workflow in action

---

### Demo 7: Concurrent Workflows 🔄

**What you'll learn:**
- Parallel agent execution
- Fan-out and fan-in patterns
- Aggregating results from multiple agents

**Description:**
Harness the power of parallel processing! This demo shows how to run multiple agents concurrently and aggregate their results. A physicist and chemist agent both analyze the same question simultaneously, then their responses are combined for a comprehensive answer.

**Key Features:**
- 🔄 Concurrent agent execution
- 📊 Result aggregation
- ⚡ Parallel processing
- 🎯 Specialized expert agents

**Setup:**
- Navigate to `src/Demo007_WorkflowConcurrentDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Run `dotnet run` or press F5 in Visual Studio

**Try questions like:**
- "What is water?" (Get both physics and chemistry perspectives)
- "Explain combustion" (See different expert viewpoints)
- "How does photosynthesis work?" (Multiple expert analyses)

---

### Demo 8: Workflow Handoffs 🤝

**What you'll learn:**
- Implementing agent handoff patterns
- Building triage and routing systems
- Creating specialized agent teams

**Description:**
Create intelligent agent teams! This demo implements a sophisticated handoff system where a triage agent analyzes questions and routes them to appropriate specialist agents (history or math tutors). Learn how to build smart routing and delegation systems.

**Key Features:**
- 🤝 Intelligent agent handoffs
- 🎯 Automatic triage and routing
- 👥 Specialized agent teams
- 🧠 Context-aware delegation

**Setup:**
- Navigate to `src/Demo008_WorkflowHandoffDemo/C#/`
- Ensure you have ran the `setup\setup.ps1` to configure your `user-secrets`
- Run `dotnet run` or press F5 in Visual Studio

**Try different types of questions:**
- History: "What caused World War I?"
- Math: "Solve this quadratic equation: x² + 5x + 6 = 0"
- Watch how the triage agent routes to the correct specialist!

---

### Demo 9: Development UI (Python) 🐍

**What you'll learn:**
- Using Agent Framework with Python
- Setting up a web-based development UI for testing agents
- Creating simple workflows with executors
- Working with Python environment management using UV

**Description:**
Explore the Agent Framework using Python! This demo demonstrates how to use the Agent Framework DevUI to create and test agents with a web-based interface. It includes weather and general-purpose agents, plus a simple text transformation workflow that converts text to uppercase and adds exclamation marks.

**Key Features:**
- 🐍 Python implementation of Agent Framework
- 🌐 Web-based development UI
- 🌤️ Weather assistant with tool functions
- 💬 General conversational agent
- ⚡ Simple text transformation workflow
- 🔧 Easy agent testing interface

**Setup:**
- Navigate to `src/Demo009_DevUI/python/`
- Install UV package manager:
   - Windows: `powershell -ExecutionPolicy ByPass -c "irm https://astral.sh/uv/install.ps1 | iex"`
   - macOS/Linux (curl): `curl -fsSL https://astral.sh/uv/install.sh | bash`
   - macOS/Linux (wget): `wget -qO- https://astral.sh/uv/install.sh | bash`
   - using pip: `pip install uv`
- Copy `.env.sample` to `.env` and configure your Azure OpenAI settings
- Install dependencies: `uv sync`
- Run the demo: `uv run --env-file .env python main.py`
- Access the UI at http://localhost:8090

**Try these interactions:**
- Weather queries: "What's the weather in Seattle?"
- Time queries: "What time is it in Lisbon?"
- Test the workflow with any text input
- Explore the web interface for easy agent testing

---

### Demo 10: DevUI Workflow Demo (Python) 🔀

**What you'll learn:**
- Building complex branching workflows in Python
- Implementing conditional routing based on agent responses
- Creating content review workflows with quality control
- Using structured outputs for decision making

**Description:**
Master advanced workflow patterns with Python! This demo showcases a sophisticated content review workflow where a writer creates content, a reviewer evaluates it, and based on the quality score, the content either goes directly to publication or through an editing process first. Both paths converge at a final summarizer.

**Key Features:**
- 🔀 Conditional workflow branching
- 📝 Multi-agent content creation pipeline
- ⚖️ Quality-based routing decisions
- 📊 Structured output evaluation
- 🔄 Workflow path convergence
- 📋 Automated content review process

**Setup:**
- Navigate to `src/Demo010_DevUI_WorkflowDemo/python/`
- Install UV package manager:
   - Windows: `powershell -ExecutionPolicy ByPass -c "irm https://astral.sh/uv/install.ps1 | iex"`
   - macOS/Linux (curl): `curl -fsSL https://astral.sh/uv/install.sh | bash`
   - macOS/Linux (wget): `wget -qO- https://astral.sh/uv/install.sh | bash`
   - using pip: `pip install uv`
- Copy `.env.sample` to `.env` and configure your Azure OpenAI settings
- Install dependencies: `uv sync`
- Run the demo: `uv run --env-file .env python main.py`
- Access the UI at http://localhost:8093

**Workflow Paths:**
- **High Quality (score ≥ 80):** Writer → Reviewer → Publisher → Summarizer
- **Low Quality (score < 80):** Writer → Reviewer → Editor → Publisher → Summarizer
- Both paths converge at the Summarizer for final reporting

**Try content creation prompts:**
- "Write an article about renewable energy"
- "Create a guide for Python beginners"
- "Write a product description for a smart watch"
- Watch how different quality scores route through different workflow paths!

---

## Setup Instructions 🛠️

### Prerequisites

1. **Clone the repository:**
   ```powershell
   git clone https://github.com/marconsilva/Agent-Framework-Demo.git
   cd Agent-Framework-Demo
   ```

2. **For C# Demos - Install .NET 9.0 SDK:**
   - Download and install from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

3. **For .NET Aspire (Recommended for C# Demos):**
   - Install .NET Aspire workload: `dotnet workload install aspire`
   - This enables running multiple demos simultaneously with a unified dashboard

4. **For Python Demos - Install Python and UV:**
   - **Python 3.12+:** Download from [https://python.org](https://python.org)
   - **UV Package Manager:** Install with `pip install uv`

5. **Install Visual Studio or VS Code:**
   - **Visual Studio:** Download from [https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/)
   - **VS Code:** Download from [https://code.visualstudio.com/](https://code.visualstudio.com/) with C# and Python extensions

6. **Azure OpenAI Setup:**
   - Create an Azure OpenAI resource in the Azure portal
   - Deploy a GPT-4 model (recommended: gpt-4o)
   - Note your endpoint

7. **Environment Configuration:**

   **For C# Demos:**
   - Create your `user-secrets` settings for each project
   - Add your `config.ps1` file to the setup folder based on the `config.ps1.sample` file:
     ```
      $AZURE_OPENAI_ENDPOINT = "https://your-azure-openai-endpoint.openai.azure.com/"
      $AZURE_OPENAI_MODEL_ID = "gpt-4o"
      $GITHUB_PERSONAL_ACCESS_TOKEN = "{YOUR_GITHUB_PAT}"
     ```
   - Now run the `setup.ps1` script and it will initialize and set the `user-secrets` on each project.
   - You can update the `config.ps1` any time and re-run the `setup.ps1` file as many times as you want.

   **For Python Demos:**
   - Navigate to each Python demo folder (`Demo009_DevUI/python/` or `Demo010_DevUI_WorkflowDemo/python/`)
   - Copy `.env.sample` to `.env` in each folder
   - Update the `.env` file with your Azure OpenAI settings:
     ```
     AZURE_OPENAI_ENDPOINT="https://your-azure-openai-endpoint.openai.azure.com/"
     AZURE_OPENAI_CHAT_DEPLOYMENT_NAME="gpt-4o-demo"
     AZURE_OPENAI_API_VERSION="2025-01-01-preview"
     AZURE_OPENAI_API_KEY="your_azure_openai_api_key_here"
     ```

### Additional Requirements for Specific Demos

**Demo 4 (MCP GitHub):**
- Install Docker or Podman for containerized MCP server
- Generate a GitHub Personal Access Token
- Add to `setup\config.ps1` file: `$GITHUB_PERSONAL_ACCESS_TOKEN=your-token-here`

---

## Running with .NET Aspire 🚀

### What is .NET Aspire?

.NET Aspire provides a unified development experience for running multiple applications simultaneously. For this demo project, Aspire allows you to launch several Agent Framework demos at once and monitor them through a single, elegant dashboard.

### Aspire-Enabled Demos

The following demos have been configured to work with .NET Aspire:
- ✅ **Demo 1:** Joker Agent 🃏
- ✅ **Demo 2:** Light Plugin 💡  
- ✅ **Demo 3:** Human in the Loop 👥
- ✅ **Demo 6:** Sequential Workflows ⚡
- ✅ **Demo 7:** Concurrent Workflows 🔄

### Running Multiple Demos with Aspire

**Prerequisites:**
- Ensure you've completed the setup instructions above
- Install .NET Aspire workload: `dotnet workload install aspire`

**Steps:**

1. **Navigate to the Aspire solution:**
   ```powershell
   cd src/aspire
   ```

2. **Launch all demos with Aspire:**
   ```powershell
   dotnet run --project AgentFramework.AppHost
   ```
   
   Or alternatively:
   ```powershell
   aspire run
   ```

3. **Access the Aspire Dashboard:**
   - The Aspire dashboard will automatically open in your browser
   - If not, navigate to the URL displayed in the console (typically `https://localhost:15000`)

4. **Explore Multiple Demos:**
   - All 5 Aspire-enabled demos will be available in the dashboard
   - Click on any demo to access its individual interface
   - Monitor logs, metrics, and traces across all demos from one place
   - Switch between demos seamlessly without stopping and starting individual projects

### Benefits of Using Aspire

- 🎯 **Unified Dashboard:** Monitor all demos from a single interface
- 📊 **Observability:** View logs, metrics, and traces across all applications
- ⚡ **Quick Access:** Jump between different demos instantly
- 🔍 **Resource Monitoring:** Track CPU, memory, and other resources
- 🚀 **Developer Experience:** Simplified setup and management

### Individual Demo Execution

If you prefer to run demos individually:

**C# Demos (Demos 1-8):**
1. Navigate to any demo folder: `src/Demo00X_DemoName/C#/`
2. Restore packages: `dotnet restore`
3. Run the demo: `dotnet run` or press F5 in Visual Studio
4. Follow the on-screen prompts and enjoy exploring! 🎉

**Python Demos (Demos 9-10):**
1. Navigate to the Python demo folder: `src/Demo00X_DemoName/python/`
2. Install dependencies: `uv sync`
3. Run the demo: `uv run --env-file .env python main.py`
4. Access the web UI at the provided localhost URL
5. Interact with agents and workflows through the browser interface! 🐍

---

## Contributing 🤝

Contributions are welcome! Please read the [CONTRIBUTING.md](CONTRIBUTING.md) file for guidelines on how to contribute to this project.

---

## License 📄

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## 🤖 About This README

*This README.md file was collaboratively created using GitHub Copilot and reviewed by a human to ensure accuracy and clarity. Embracing the future of human-AI collaboration! 🚀*