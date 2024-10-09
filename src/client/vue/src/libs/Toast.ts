import { ElMessage } from 'element-plus';

class Toast {
  static success(message: string) {
    ElMessage({
      message,
      type: 'success',
    });
  }

  static error(message: string) {
    ElMessage({
      message,
      type: 'error'
    });
  }

  static warning(message: string) {
    ElMessage({
      message,
      type: 'warning'
    });
  }

  static info(message: string) {
    ElMessage({
      message,
      type: 'info'
    });
  }
}

export const toast = Toast;