import { memo } from 'react';
import { EdgeLabelRenderer } from '@xyflow/react';
import { EdgeData } from '../types';

interface CustomEdgeProps {
  id: string;
  sourceX: number;
  sourceY: number;
  targetX: number;
  targetY: number;
  sourcePosition: any;
  targetPosition: any;
  style: any;
  data?: EdgeData;
}

const CustomEdge = memo(({ 
  id, 
  sourceX, 
  sourceY, 
  targetX, 
  targetY, 
  sourcePosition, 
  targetPosition, 
  style = {}, 
  data 
}: CustomEdgeProps) => {
  return (
    <>
      <path
        id={id}
        style={{
          ...style,
          stroke: '#666',
          strokeWidth: 2,
        }}
        className="react-flow__edge-path"
        d={`
          M ${sourceX} ${sourceY}
          C ${sourceX} ${(sourceY + targetY) / 2}
          ${targetX} ${(sourceY + targetY) / 2}
          ${targetX} ${targetY}
        `}
      />
      {data?.sourceSignal && (
        <EdgeLabelRenderer>
          <div
            style={{
              position: 'absolute',
              transform: `translate(-50%, -50%) translate(${targetX}px,${targetY}px)`,
              background: '#fff',
              padding: 4,
              borderRadius: 4,
              fontSize: 10,
              color: '#333',
              border: '1px solid #ddd',
            }}
          >
            {data.sourceSignal}
          </div>
        </EdgeLabelRenderer>
      )}
    </>
  );
});

export default CustomEdge;
