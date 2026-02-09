using System;
using System.Collections.Generic;
using System.Text;
using GENE.JoeRoom;

namespace GENE.Basic.Nodes.SmartThings {
    public class SmartThingsBulb(OAuthManager OAuth, string name, string id)
        : SmartThingsDevice(name, id), ISmartDimmable {
        public void Toggle(bool enabled)
        {
            Signal(
                new(OAuth.GetToken().Token, new SmartThingsCommand
                {
                    component = Name,
                    capability = "switch",
                    command = enabled ? "on" : "off",
                })
            );
        }

        public void SetLevel(byte brightness)
        {
            Signal(
                new(OAuth.GetToken().Token, new SmartThingsCommand
                {
                    component = Name,
                    capability = "switchLevel",
                    command = "setLevel",
                    arguments = [brightness]
                })
            );
        }
    }
}