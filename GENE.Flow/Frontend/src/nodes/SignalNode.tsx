import {memo} from 'react';
import {NodeResizer, Position} from '@xyflow/react';
import {NodeData, SignalTypes} from '../types';
import {CreateTitle, Handles, Labels} from "./NodeDefaults";

interface SignalNodeProps {
    data: NodeData;
    selected: boolean;
}

const SignalNode = ({ data, selected }: SignalNodeProps) => {
    return (
        <>
            <NodeResizer minWidth={150} minHeight={80} isVisible={selected} />

            {Handles(data.signals, {type: SignalTypes.SIGNAL})}

            <div style={{ padding: 10, minWidth: 150 }}>
                {CreateTitle(data)}

                {Labels(data.signals)}
            </div>
        </>
    );
};

export default memo(SignalNode);
