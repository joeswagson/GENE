import { useCallback, useState, type ChangeEventHandler } from 'react';
import {
  ReactFlow,
  Background,
  Controls,
  MiniMap,
  useNodesState,
  useEdgesState,
  reconnectEdge,
  addEdge,
  Position,
  MarkerType,
  useNodes, ColorMode, Panel, StraightEdge, SmoothStepEdge,
} from '@xyflow/react';

import '@xyflow/react/dist/style.css';
import './main.css'

import { Header } from './static/Header';

import ResizableNode from './nodes/ResizableNode';
import ResizableNodeSelected from './nodes/ResizableNodeSelected';
import CustomResizerNode from './nodes/CustomResizerNode';
import { Globals } from './static/global';


const nodeTypes = {
  ResizableNode,
  ResizableNodeSelected,
  CustomResizerNode,
};

const edgeTypes = {
  default: SmoothStepEdge
  custom: CustomEdge,
}

const nodeDefaults = {
  sourcePosition: Position.Right,
  targetPosition: Position.Left,
};

const initialNodes = [
  {
    id: '1',
    type: 'ResizableNodeSelected',
    position: { x: 0, y: 150 },
    data: { label: 'default style 1' },
    ...nodeDefaults,
  },
  {
    id: '2',
    type: 'ResizableNodeSelected',
    position: { x: 250, y: 0 },
    data: { label: 'default style 2' },
    ...nodeDefaults,
  },
  {
    id: '3',
    type: 'ResizableNodeSelected',
    position: { x: 250, y: 150 },
    data: { label: 'default style 3' },
    ...nodeDefaults,
  },
  {
    id: '4',
    type: 'ResizableNodeSelected',
    position: { x: 250, y: 300 },
    data: { label: 'default style 4' },
    ...nodeDefaults,
  },
];
const initialEdges = [
  {
    id: 'e1-2',
    source: '1',
    target: '2',
    animated: true,
    markerEnd: { type: MarkerType.ArrowClosed },
  },
  {
    id: 'e1-3',
    source: '1',
    target: '3',
    markerEnd: { type: MarkerType.ArrowClosed },
  },
  {
    id: 'e1-4',
    source: '1',
    target: '4',
    markerEnd: { type: MarkerType.ArrowClosed },
  },
];

const App = () => {
  const LOCALS = {
    snapGrid: 20
  };
  
  const [colorMode, setColorMode] = useState<ColorMode>('dark');
  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);
  
  const onChange: ChangeEventHandler<HTMLSelectElement> = (evt) => {
    setColorMode(evt.target.value as ColorMode);
  };

  const onReconnect = useCallback((oldEdge, newConnection) => {
    setEdges((eds) => reconnectEdge(oldEdge, newConnection, eds));
  }, [setEdges]);

  const onConnect = useCallback((connection) => {
    setEdges((eds) => addEdge(connection, eds));
  }, [setEdges]);

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
        snapGrid={[LOCALS.snapGrid, LOCALS.snapGrid]}
        colorMode={colorMode}
        snapToGrid
        fitView
      >
        <Controls />
        <Background />

        <Panel position="top-right">
          <select
              className="xy-theme__select"
              onChange={onChange}
              data-testid="colormode-select"
          >
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