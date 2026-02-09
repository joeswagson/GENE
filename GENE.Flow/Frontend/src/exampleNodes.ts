import { MarkerType } from '@xyflow/react';

export const exampleNodes = [
  // Web Server (event source)
  {
    id: '1',
    type: 'SignalNode',
    position: { x: 0, y: 140 },
    data: {
      label: 'Web Server',
      signals: [
        { name: '/tv', type: 'object', nullable: false },
        { name: '/light', type: 'object', nullable: false },
      ],
      nodeId: 'web-server',
    },
  },

  // Activator (TV)
  {
    id: '2',
    type: 'BridgeNode',
    position: { x: 260, y: 60 },
    data: {
      label: 'Activator (TV)',
      signals: [
        { name: 'event', type: 'object', nullable: false },
      ],
      outputs: [
        { name: 'action', type: 'object', nullable: false },
      ],
      nodeId: 'activator-tv',
    },
  },

  // Activator (Bulb)
  {
    id: '3',
    type: 'BridgeNode',
    position: { x: 260, y: 220 },
    data: {
      label: 'Activator (Light)',
      signals: [
        { name: 'event', type: 'object', nullable: false },
      ],
      outputs: [
        { name: 'action', type: 'object', nullable: false },
      ],
      nodeId: 'activator-light',
    },
  },

  // SmartThingsAction
  {
    id: '4',
    type: 'BridgeNode',
    position: { x: 520, y: 140 },
    data: {
      label: 'SmartThingsAction',
      signals: [
        { name: 'set_Token', type: 'string', nullable: false },
        { name: 'set_Commands', type: 'array', nullable: false },
      ],
      outputs: [
        { name: 'get_Commands', type: 'array', nullable: false },
      ],
      nodeId: 'st-action',
    },
  },

  // SmartThingsTV
  {
    id: '5',
    type: 'BridgeNode',
    position: { x: 780, y: 40 },
    data: {
      label: 'SmartThingsTV',
      signals: [
        { name: 'Toggle', type: 'boolean', nullable: false },
        { name: 'SetVolume', type: 'number', nullable: false },
        { name: 'LaunchApp', type: 'string', nullable: false },
      ],
      outputs: [],
      nodeId: 'st-tv',
    },
  },

  // SmartThingsBulb
  {
    id: '6',
    type: 'BridgeNode',
    position: { x: 780, y: 240 },
    data: {
      label: 'SmartThingsBulb',
      signals: [
        { name: 'Toggle', type: 'boolean', nullable: false },
        { name: 'SetLevel', type: 'number', nullable: false },
      ],
      outputs: [],
      nodeId: 'st-bulb',
    },
  },

  // WebResponse
  {
    id: '7',
    type: 'OutputNode',
    position: { x: 1040, y: 140 },
    data: {
      label: 'WebResponse',
      outputs: [
        { name: 'get_StatusCode', type: 'number', nullable: false },
        { name: 'get_RawJson', type: 'string', nullable: false },
      ],
      nodeId: 'web-response',
    },
  },
];

export const exampleEdges = [
  
];