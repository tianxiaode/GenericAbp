export function removeDuplicates(arr: any[]): any[] {
  if (!Array.isArray(arr)) {
    throw new Error('Input must be an array');
  }
  return [...new Set(arr)];
}

export function removeUndefined(arr: any[]): any[] {
  if (!Array.isArray(arr)) {
    throw new Error('Input must be an array');
  }
  return arr.filter((item) => item !== undefined);
}

export function removeNull(arr: any[]): any[] {
  if (!Array.isArray(arr)) {
    throw new Error('Input must be an array');
  }
  return arr.filter((item) => item !== null);
}

export function removeEmptyString(arr: any[]): any[] {
  if (!Array.isArray(arr)) {
    throw new Error('Input must be an array');
  }
  return arr.filter((item) => item !== '');
}

// 增加一个通用的函数来处理多种情况
export function removeItems(arr: any[], ...itemsToRemove: any[]): any[] {
  if (!Array.isArray(arr)) {
    throw new Error('Input must be an array');
  }
  return arr.filter((item) => !itemsToRemove.includes(item));
}
