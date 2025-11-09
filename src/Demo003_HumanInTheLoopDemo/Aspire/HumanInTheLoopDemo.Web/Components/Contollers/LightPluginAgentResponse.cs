using Microsoft.Extensions.AI;

namespace HumanInTheLoopDemo.Web.Components.Contollers
{
    public class LightPluginAgentResponse
    {
        public string? Response { get; set; }
        public AgentResponseType ResponseType { get; set; }
        public IEnumerable<FunctionApprovalRequestContent>? FunctionCallsToApprove { get; internal set; }
    }

    public enum AgentResponseType
    {
        Text,
        ApprovalRequest
    }
}
