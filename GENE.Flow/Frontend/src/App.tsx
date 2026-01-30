import { useCallback, useState, type ChangeEventHandler } from "react";
import {
    ReactFlow,
    Background,
    Controls,
    Panel,
    addEdge,
    reconnectEdge,
    useNodesState,
    useEdgesState,
    Position,
    SmoothStepEdge,
} from "@xyflow/react";

import "@xyflow/react/dist/style.css";
import "./main.css";

import { Header } from "./static/Header";
import { SignalNode, OutputNode, BridgeNode } from "./nodes/Node";
import { exampleNodes, exampleEdges } from "./exampleNodes";
import { NodeData, EdgeData, DataType } from "./types";
import { Globals } from "./static/global";

// --- Node + edge types ---
const nodeTypes = { SignalNode, OutputNode, BridgeNode };
const edgeTypes = { default: SmoothStepEdge };

// --- Type compatibility ---
const canConnect = (from: DataType, to: DataType) => {
    if (from === to) return true;
    const bridges = new Set(["string->boolean", "number->string", "object->boolean"]);
    return bridges.has(`${from}->${to}`);
};

// --- Helpers ---
function getHandleType(nodeId: string, handleId: string, nodes: NodeData[]): DataType | null {
    const node = nodes.find((n) => n.nodeId === nodeId);
    if (!node) return null;
    const all = [...(node.signals ?? []), ...(node.outputs ?? [])];
    const method = all.find((m) => `${m.name}` === handleId.split("-").pop());
    return method?.type ?? null;
}

// --- App ---
const App = () => {
    const [nodes, setNodes, onNodesChange] = useNodesState(exampleNodes);
    const [edges, setEdges, onEdgesChange] = useEdgesState(exampleEdges);
    const [colorMode, setColorMode] = useState<"dark" | "light" | "system">("dark");

    const onChange: ChangeEventHandler<HTMLSelectElement> = (evt) =>
        setColorMode(evt.target.value as "dark" | "light" | "system");

    const onReconnect = useCallback(
        (oldEdge: any, newConnection: any) => setEdges((eds) => reconnectEdge(oldEdge, newConnection, eds)),
        []
    );

    const onConnect = useCallback(
        (connection: any) => {
            const fromType = getHandleType(connection.source, connection.sourceHandle, nodes);
            const toType = getHandleType(connection.target, connection.targetHandle, nodes);
            const valid = fromType !== null && toType !== null && canConnect(fromType, toType);

            setEdges((eds) =>
                addEdge(
                    {
                        ...connection,
                        data: { valid },
                        style: valid
                            ? undefined
                            : { stroke: "#ff4d4f", strokeDasharray: "4 4" },
                    },
                    eds
                )
            );
        },
        [nodes]
    );

    const isValidConnection = useCallback(
        (connection: any) => {
            const fromType = getHandleType(connection.source, connection.sourceHandle, nodes);
            const toType = getHandleType(connection.target, connection.targetHandle, nodes);
            return fromType !== null && toType !== null && canConnect(fromType, toType);
        },
        [nodes]
    );

    Globals.Nodes = nodes;

    return (
        <>
            <Header />
            <ReactFlow
                nodes={nodes}
                edges={edges}
                nodeTypes={nodeTypes}
                edgeTypes={edgeTypes}
                onNodesChange={onNodesChange}
                onEdgesChange={onEdgesChange}
                onReconnect={onReconnect}
                onConnect={onConnect}
                isValidConnection={isValidConnection}
                snapGrid={[20, 20]}
                snapToGrid
                colorMode={colorMode}
                fitView
            >
                <Controls />
                <Background />
                <Panel position="top-right">
                    <select className="xy-theme__select" onChange={onChange} data-testid="colormode-select">
                        <option value="dark">dark</option>
                        <option value="light">light</option>
                        <option value="system">system</option>
                    </select>
                </Panel>
            </ReactFlow>
        </>
    );
};

export { Globals };
export default App;
