import { ErrorExtractor, ErrorExtractorConfig } from '../types'
import { AbpErrorExtractor } from './AbpErrorExtractor';
import { SpringBootErrorExtractor } from './SpringBootErrorExtractor';
import { NestJSErrorExtractor } from './NestJSErrorExtractor';
import { ConfigurableErrorExtractor } from './ConfigurableErrorExtractor';
/**
 * 组合错误提取器 - 按顺序尝试多个提取器
 */
export class CompositeErrorExtractor implements ErrorExtractor {
    private extractors: ErrorExtractor[];

    constructor(extractors: ErrorExtractor[] = []) {
        this.extractors = extractors.length > 0
            ? extractors
            : [
                new AbpErrorExtractor(),
                new NestJSErrorExtractor(),
                new SpringBootErrorExtractor(),
                new ConfigurableErrorExtractor()
            ];
    }

    canHandle(data: any, status: number): boolean {
        return this.extractors.some(extractor => extractor.canHandle(data, status));
    }

    extractMessage(data: any, status: number): string {
        for (const extractor of this.extractors) {
            if (extractor.canHandle(data, status)) {
                return extractor.extractMessage(data, status);
            }
        }
        return new ConfigurableErrorExtractor().extractMessage(data, status);
    }

    extractCode(data: any, status: number): number {
        for (const extractor of this.extractors) {
            if (extractor.canHandle(data, status)) {
                return extractor.extractCode(data, status);
            }
        }
        return new ConfigurableErrorExtractor().extractCode(data, status);
    }

    extractValidationErrors(data: any, status: number): any {
        for (const extractor of this.extractors) {
            if (extractor.canHandle(data, status)) {
                return extractor.extractValidationErrors(data, status);
            }
        }
        return new ConfigurableErrorExtractor().extractValidationErrors(data, status);
    }

    extractRawError(data: any, status: number): any {
        for (const extractor of this.extractors) {
            if (extractor.canHandle(data, status)) {
                return extractor.extractRawError(data, status);
            }
        }
        return new ConfigurableErrorExtractor().extractRawError(data, status);
    }
}