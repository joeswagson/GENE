import {Handle, Position} from "@xyflow/react";
import {HandlesData, MethodDefinition, NodeData, SignalType, SignalTypes} from "../types";
import {handleY} from "./YHandler";

const generalStyle = {}

const handleStyle = {
    ...generalStyle
}

const labelStyle = {
    ...generalStyle
}

const titleStyle = {
    ...generalStyle,

    fontSize: 12,
    marginBottom: 8,
    fontWeight: 'bold',
}

function CreateHandle(method: MethodDefinition,
                      i: number,
                      data: HandlesData) {
    const signal = data.type == SignalTypes.SIGNAL;
    return (<Handle
        key={method.name}
        id={`${data.type}-${method.name}`}
        type={signal ? 'source' : 'target'}
        position={method.position ?? signal ? Position.Left : Position.Right}
        style={{...handleStyle, top: handleY(i)}}
    />);
}

function CreateLabel(method: MethodDefinition, i: number) {
    return (
        <div key={method.name} style={{...labelStyle, top: handleY(i)}}>
            <code>{method.name}</code> ({method.type})
        </div>
    );
}

export function CreateTitle(data: NodeData) {
    return (
        <div style={titleStyle}>
            {data.label}
        </div>
    );
}

export function Handles(methods: MethodDefinition[], data: HandlesData) {
    return methods.map((m, i) => CreateHandle(m, i, data));
}

export function Labels(methods: MethodDefinition[]) {
    return methods.map((m, i) => CreateLabel(m, i));
}