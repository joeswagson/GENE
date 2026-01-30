// Type definitions matching the Typist.cs structure

import {Position} from "@xyflow/react";

export type SignalType = 'signal' | 'output';
export enum SignalTypes {
  SIGNAL = 'signal',
  OUTPUT = 'output'
}

export interface MethodDefinition {
  name: string;
  type: string;
  nullable?: boolean;
  position?: Position;
}

export interface HandlesData {
  type: SignalType,
  position?: Position
}

export interface NodeDefinition {
  name: string;
  signals: MethodDefinition[];
  outputs: MethodDefinition[];
}

export interface NodeData {
  label: string;
  signals?: MethodDefinition[];
  outputs?: MethodDefinition[];
  nodeId: string;
}

export interface EdgeData {
  sourceSignal?: string;
  targetSignal?: string;
}

// Type definitions for different data types
export type DataType =
  | 'boolean'
  | 'number'
  | 'string'
  | 'object'
  | 'array'
  | 'tuple'
  | 'void';

export interface TypeDefinition {
  name: string;
  friendlyName: string;
  nullable?: boolean;
}

export interface FunctionType {
  parameters: MemberDefinition[];
  returnType?: TypeDefinition;
}

export interface MemberDefinition {
  name: string;
  type: TypeDefinition;
}

export interface Getter extends MemberDefinition {
  kind: 'getter';
}

export interface Setter extends MemberDefinition {
  kind: 'setter';
}

export interface Method extends MemberDefinition {
  kind: 'method';
  parameters: MemberDefinition[];
  returnType?: TypeDefinition;
}
