using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Basic.Nodes.SmartThings {
    public class SmartThingsBulb(SmartThingsToken token, string name, string id)
        : SmartThingsDevice(name, id), ISmartDimmable {
        private SmartThingsToken Auth = token;

        public void UpdateToken(SmartThingsToken token) => Auth = token;

        public void Toggle(bool enabled)
        {
            Signal(
                new SmartThingsAction(Auth.Token, new SmartThingsCommand
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
                new SmartThingsAction(Auth.Token, new SmartThingsCommand
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