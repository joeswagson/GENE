import {memo} from 'react';
import {Handle, Position, NodeResizer} from '@xyflow/react';
import {NodeData, SignalTypes} from '../types';
import {handleY} from './YHandler';
import {CreateTitle, Handles, Labels} from "./NodeDefaults";

interface OutputNodeProps {
    data: NodeData;
    selected: boolean;
}

const OutputNode = ({data, selected}: OutputNodeProps) => {
    return (
        <>
            <NodeResizer minWidth={160} minHeight={40} isVisible={selected}/>

            {Handles(data.outputs, {type: SignalTypes.OUTPUT})}

            <div style={{padding: 10, minWidth: 160}}>
                {CreateTitle(data)}

                {Labels(data.outputs)}
            </div>
        </>
    );
};

export default memo(OutputNode);
