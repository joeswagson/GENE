import {useCallback, useState, type ChangeEventHandler} from "react";
import {
    Node,
    ReactFlow,
    Background,
    Controls,
    Panel,
    addEdge,
    reconnectEdge,
    useNodesState,
    useEdgesState,
    Position,
    SmoothStepEdge, BackgroundVariant,
} from "@xyflow/react";

import "@xyflow/react/dist/style.css";
import "./main.css";

import {Header} from "./static/Header";
import {SignalNode, OutputNode, BridgeNode} from "./nodes/Node";
import {exampleNodes, exampleEdges} from "./exampleNodes";
import {NodeData, EdgeData, DataType} from "./types";
import {Globals} from "./static/global";

// --- Node + edge types ---
const nodeTypes = {SignalNode, OutputNode, BridgeNode};
const edgeTypes = {default: SmoothStepEdge};

// --- Type compatibility ---
const canConnect = (from: DataType, to: DataType) => {
    if (from === to) return true;
    const bridges = new Set(["string->boolean", "number->string", "object->boolean"]);
    return bridges.has(`${from}->${to}`);
};

// --- Helpers ---
function getHandleType(
    nodeId: string, // this is React Flow node.id
    handleId: string,
    nodes: Node<NodeData>[]
): DataType | null {
    const node = nodes.find((n) => n.id === nodeId);
    if (!node) {
        console.log(`[getHandleType] Node not found: ${nodeId}`);
        return null;
    }

    const { signals = [], outputs = [] } = node.data;
    const methodName = handleId.split("-").pop();
    const method = [...signals, ...outputs].find((m) => m.name === methodName);

    if (!method) {
        console.log(`[getHandleType] Handle not found: ${handleId} on node ${node.data.nodeId}`);
        return null;
    }

    return method.type ?? null;
}



// --- App ---
const App = () => {
    const [nodes, setNodes, onNodesChange] = useNodesState(exampleNodes);
    const [edges, setEdges, onEdgesChange] = useEdgesState(exampleEdges);
    const [colorMode, setColorMode] = useState<"dark" | "light" | "system">("dark");
    const [pattern, setPattern] = useState<"lines" | "dots" | "cross">("lines");

    const onChange: ChangeEventHandler<HTMLSelectElement> = (e) => {
        if (e.target.id === "colormode-select") {
            setColorMode(e.target.value as typeof colorMode);
        } else if (e.target.id === "pattern-select") {
            setPattern(e.target.value as typeof pattern);
        }
    };


    const onReconnect = useCallback(
        (oldEdge: any, newConnection: any) => setEdges((eds) => reconnectEdge(oldEdge, newConnection, eds)),
        []
    );

    const onConnect = useCallback(
        (connection: any) => {
            console.log("[onConnect] Attempting connection:", connection);
            const fromType = getHandleType(connection.source, connection.sourceHandle, nodes);
            const toType = getHandleType(connection.target, connection.targetHandle, nodes);
            const valid = fromType !== null && toType !== null && canConnect(fromType, toType);
            console.log(`[onConnect] fromType=${fromType}, toType=${toType}, valid=${valid}`);

            setEdges((eds) =>
                addEdge(
                    {
                        ...connection,
                        data: {valid},
                        style: valid
                            ? undefined
                            : {stroke: "#ff4d4f", strokeDasharray: "4 4"},
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
            <Header/>
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
                <Controls/>
                <Background
                    gap={20}
                    variant={pattern === "dots"
                        ? BackgroundVariant.Dots
                        : pattern === "cross"
                            ? BackgroundVariant.Cross
                            : BackgroundVariant.Lines}
                    color={colorMode === "dark" ? "#2a2a2a" : "#e5e7eb"}
                    bgColor={colorMode === "dark" ? "#121212" : "#ffffff"}
                />
                <Panel position="top-right" style={{display: "flex", flexDirection: "column", padding: "5px"}}>
                    <select className="xy-theme__select" onChange={onChange} id="colormode-select">
                        <option value="dark">dark</option>
                        <option value="light">light</option>
                        <option value="system">system</option>
                    </select>
                    <br/>
                    <select className="xy-theme__select" onChange={onChange} id="pattern-select">
                        <option value="lines">lines</option>
                        <option value="dots">dots</option>
                        <option value="cross">cross</option>
                    </select>
                </Panel>
            </ReactFlow>
        </>
    );
};

export {Globals};
export default App;
