export const HANDLE_SPACING = 24;
export const HANDLE_OFFSET = 30;

export const handleY = (index: number, headerHeight: number = 25) =>
    headerHeight + HANDLE_OFFSET + index * HANDLE_SPACING;
