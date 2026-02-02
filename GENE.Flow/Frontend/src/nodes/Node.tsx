import { memo } from "react";
import { NodeResizer } from "@xyflow/react";
import { NodeData, SignalTypes } from "../types";
import { CreateTitle, Handles, Labels } from "./NodeDefaults";

interface NodeProps {
    data: NodeData;
    selected: boolean;
}

export const SignalNode = memo(({ data, selected }: NodeProps) => (
    <div className="node-container">
        <NodeResizer minWidth={150} minHeight={120} isVisible={selected} />
        {Handles(data.signals ?? [], { type: SignalTypes.SIGNAL })}
        <div style={{ padding: 10, minWidth: 150 }}>
            {CreateTitle(data)}
        </div>
        <div className="node-signals">
            {Labels(data.signals ?? [])}
        </div>
    </div>
));

export const OutputNode = memo(({ data, selected }: NodeProps) => (
    <div className="node-container">
        <NodeResizer minWidth={160} minHeight={120} isVisible={selected} />
        {Handles(data.outputs ?? [], { type: SignalTypes.OUTPUT })}
        <div style={{ padding: 10, minWidth: 160 }}>
            {CreateTitle(data)}
        </div>
        <div className="node-outputs">
            {Labels(data.outputs ?? [])}
        </div>
    </div>
));

export const BridgeNode = memo(({ data, selected }: NodeProps) => (
    <div className="bridge-node-container">
        <NodeResizer minWidth={160} minHeight={120} isVisible={selected} />
        {Handles(data.signals ?? [], { type: SignalTypes.SIGNAL })}
        {Handles(data.outputs ?? [], { type: SignalTypes.OUTPUT })}
        <div style={{ padding: 10, minWidth: 120 }}>{CreateTitle(data)}</div>
        <br/>
        <div className="flex" style={{alignItems: "baseline", position: "absolute", top: "40px", width: "calc(100% - 20px)", height: "calc(100% - 40px)"}}>
            <div className="flex-col flex-left node-signals">
                {Labels(data.signals ?? [])}
            </div>

            <div className="flex-col flex-right node-outputs">
                {Labels(data.outputs ?? [])}
            </div>
        </div>
    </div>
));
