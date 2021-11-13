export interface AtmContent {
    contents: Array<BillInfo>,
    total: number,
    withdrawn?: Array<BillInfo> | undefined
}

export interface BillInfo {
    value: number,
    amount: number
}
