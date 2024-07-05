import { CashFlow } from "./CashFlow";

export class DailyConsolidated{
    cashFlows: CashFlow[];
    amountDebit: number;
    amountCredit: number;
    amountTotal: number;
    date: Date;
}