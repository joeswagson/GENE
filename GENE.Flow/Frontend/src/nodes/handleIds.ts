export const makeHandleId = (
    kind: "signal" | "output",
    name: string
) => `${kind}-${name}`;