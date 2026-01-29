import { SizeProperties, StyleProperty } from "./Properties";

interface ButtonProps extends SizeProperties, StyleProperty {
    label: string;
    onClick: () => any;
}

const ContainerClass = "button-container"
const ElementalClass = "button-elemental"

export default function Button({ label, onClick, width, height, style }: ButtonProps) {
    var custom = {
        ...style,

        display: "inline-block",
        width: width,
        height: height
    }

    return (
        <div className={ContainerClass} style={custom}>
            <button className={ElementalClass} style={custom} onClick={onClick}>
                {label}
            </button>
        </div>
    );
}