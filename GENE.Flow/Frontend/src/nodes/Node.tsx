import { memo } from "react";
import { NodeResizer } from "@xyflow/react";
import { NodeData, SignalTypes } from "../types";
import { CreateTitle, Handles, Labels } from "./NodeDefaults";

interface NodeProps {
    data: NodeData;
    selected: boolean;
}

export const SignalNode = memo(({ data, selected }: NodeProps) => (
    <>
        <NodeResizer minWidth={150} minHeight={80} isVisible={selected} />
        {Handles(data.signals ?? [], { type: SignalTypes.SIGNAL })}
        <div style={{ padding: 10, minWidth: 150 }}>
            {CreateTitle(data)}
            {Labels(data.signals ?? [])}
        </div>
    </>
));

export const OutputNode = memo(({ data, selected }: NodeProps) => (
    <>
        <NodeResizer minWidth={160} minHeight={40} isVisible={selected} />
        {Handles(data.outputs ?? [], { type: SignalTypes.OUTPUT })}
        <div style={{ padding: 10, minWidth: 160 }}>
            {CreateTitle(data)}
            {Labels(data.outputs ?? [])}
        </div>
    </>
));

export const BridgeNode = memo(({ data, selected }: NodeProps) => (
    <>
        <NodeResizer minWidth={120} minHeight={60} isVisible={selected} />
        {Handles(data.signals ?? [], { type: SignalTypes.SIGNAL })}
        {Handles(data.outputs ?? [], { type: SignalTypes.OUTPUT })}
        <div style={{ padding: 10, minWidth: 120 }}>{CreateTitle(data)}</div>
    </>
));
