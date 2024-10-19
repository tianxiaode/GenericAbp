import { isEmpty } from "./String";

export class Plural {
    static irregularForms: Record<string, string> = {
        child: "children",
        man: "men",
        woman: "women",
        tooth: "teeth",
        foot: "feet",
        goose: "geese",
        mouse: "mice",
        person: "people",
    };

    static getPlural(value: number, singular: string, plural: string): string {
        return value === 1 ? singular : plural;
    }

    static getPluralWithCount(value: number, singular: string, plural: string): string {
        return `${value} ${this.getPlural(value, singular, plural)}`;
    }

    static get(value: string): string {
        if (isEmpty(value)) return "";

        // 检查不规则形式
        if (this.irregularForms[value]) {
            return this.irregularForms[value];
        }

        const len = value.length;
        
        // 处理复数形式逻辑
        if (value.endsWith("s")) return value; // 已经是复数形式

        if (value.endsWith("y") && !["ay", "ey", "iy", "oy", "uy"].includes(value.slice(-2))) {
            return value.slice(0, -1) + "ies";
        }

        if (value.endsWith("o") && ["ch", "sh", "x", "z", "s"].includes(value.slice(-2, -1))) {
            return value + "es";
        }

        if (/(s|sh|ch|x|z)$/.test(value)) {
            return value + "es";
        }

        if (value.endsWith("f")) {
            return value.slice(0, len - 1) + "ves"; // 以“f”结尾的单词
        } else if (value.endsWith("fe")) {
            return value.slice(0, len - 2) + "ves"; // 以“fe”结尾的单词
        }

        // 其他情况直接加's'
        return value + "s";
    }

    static setIrregularForms(forms: Record<string, string>): void {
        if (typeof forms !== "object" || Array.isArray(forms)) {
            throw new Error("参数必须为对象");
        }
        this.irregularForms = { ...this.irregularForms, ...forms };
    }
}
