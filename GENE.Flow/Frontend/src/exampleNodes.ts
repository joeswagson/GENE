import {MarkerType, Position} from '@xyflow/react';
import { NodeData } from './types';

export const exampleNodes = [
  {
    id: '1',
    type: 'SignalNode',
    position: { x: 0, y: 160 }, // snapped-ish
    data: {
      label: 'Input Node',
      signals: [
        { name: 'setValue', type: 'string', nullable: false },
        { name: 'toggleState', type: 'boolean', nullable: false },
      ],
      nodeId: 'input-node-1',
    },
  },
  {
    id: '2',
    type: 'OutputNode',
    position: { x: 300, y: 0 },
    data: {
      label: 'Output Node',
      outputs: [
        { name: 'value', type: 'string', nullable: true },
        { name: 'state', type: 'boolean', nullable: false },
      ],
      nodeId: 'output-node-1',
    },
  },
  {
    id: '3',
    type: 'BridgeNode',
    position: { x: 300, y: 100 },
    data: {
      label: 'Activator',
      signals: [
        { name: 'value', type: 'number', nullable: false },
        { name: 'activate', type: 'void', nullable: false },
      ],
      outputs: [
        { name: 'value', type: 'number', nullable: false },
      ],
      nodeId: 'bridge-node-1',
    },
  },
  {
    id: '4',
    type: 'BridgeNode',
    position: { x: 300, y: 160 },
    data: {
      label: 'Activator',
      signals: [
        { name: 'value', type: 'number', nullable: false },
        { name: 'activate', type: 'void', nullable: false },
      ],
      outputs: [
        { name: 'value', type: 'number', nullable: false },
      ],
      nodeId: 'bridge-node-2',
    },
  },
  {
    id: '5',
    type: 'OutputNode',
    position: { x: 600, y: 160 },
    data: {
      label: 'Final Output',
      outputs: [
        { name: 'finalValue', type: 'string', nullable: false },
      ],
      nodeId: 'output-node-2',
    },
  },
];

export const exampleEdges = [
  {
    id: 'e1-setValue-2-value',
    source: '1',
    target: '2',
    sourceHandle: 'signal-setValue',
    targetHandle: 'output-value',
    type: 'smoothstep',
    animated: true,
    markerEnd: { type: MarkerType.ArrowClosed },
  },
  {
    id: 'e1-toggleState-3-process',
    source: '1',
    target: '3',
    sourceHandle: 'signal-toggleState',
    targetHandle: 'output-process', // bridge *receives* via output side
    type: 'smoothstep',
    markerEnd: { type: MarkerType.ArrowClosed },
  },
  {
    id: 'e3-result-4-finalValue',
    source: '3',
    target: '4',
    sourceHandle: 'signal-result',
    targetHandle: 'output-finalValue',
    type: 'smoothstep',
    markerEnd: { type: MarkerType.ArrowClosed },
  },
];
