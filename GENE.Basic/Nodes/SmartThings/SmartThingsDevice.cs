using GENE.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace GENE.Basic.Nodes.SmartThings {
    public class WebResponse : NodeResponse {
        public int StatusCode;
        public string? RawJson;
    }

    public sealed class SmartThingsCommand {
        public required string component { get; init; }
        public required string capability { get; init; }
        public required string command { get; init; }
        public object[]? arguments { get; init; }
    }

    public sealed class SmartThingsCommandEnvelope {
        public required SmartThingsCommand[] commands { get; init; }
    }

    public class SmartThingsAction(string token, params SmartThingsCommand[] Commands) : NodePayload {
        public string Token { get; } = token;

        public SmartThingsCommand[] Commands = Commands;
    }
    public class SmartThingsDevice(string name, string id) : INode<SmartThingsAction, WebResponse> {
        public string Name => name;
        public string DeviceId => id;

        public WebResponse Signal(SmartThingsAction p)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", p.Token);

            var url = $"https://api.smartthings.com/v1/devices/{DeviceId}/commands";

            var envelope = new SmartThingsCommandEnvelope {
                commands = p.Commands
            };

            var json = JsonSerializer.Serialize(envelope);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync(url, content).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("stat: {0}", response.StatusCode, response.StatusCode.ToString());
            return new WebResponse {
                StatusCode = (int) response.StatusCode,
                RawJson = responseJson
            };
        }
    }
}
