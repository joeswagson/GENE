import { memo } from 'react';
import { Handle, Position, NodeResizer } from '@xyflow/react';
import {NodeData, SignalTypes} from '../types';
import { handleY } from './YHandler';
import {CreateTitle, Handles, Labels} from "./NodeDefaults";

interface BridgeNodeProps {
    data: NodeData;
    selected: boolean;
}

const BridgeNode = ({ data, selected }: BridgeNodeProps) => {
    return (
        <>
            <NodeResizer minWidth={120} minHeight={60} isVisible={selected} />

            {Handles(data.signals, {type: SignalTypes.SIGNAL})}
            {Handles(data.outputs, {type: SignalTypes.OUTPUT})}

            <div style={{ padding: 10, minWidth: 120 }}>
                {CreateTitle(data)}
            </div>
        </>
    );
};

export default memo(BridgeNode);
