import Button from "./elements/Button";
import { Globals, ReloadPage } from "./global";

export function Header() {
    return (
        <header className="flex header-flex">
            <div className="inflex flex-left header-sub">
                <h2>
                    <span>GENE </span>
                </h2>
                
                <h3 style={{ display: "inline-block", margin: "0 0" }}>
                    <strong>Flow</strong>
                </h3>

                <span>v{Globals.Version}</span>
            </div>

            <div className="flex-right header-sub">
                <Button label="Reload" width="100px" onClick={ReloadPage} />
                <Button label="Clear" width="100px" onClick={ReloadPage} />
            </div>
        </header>
    );
}