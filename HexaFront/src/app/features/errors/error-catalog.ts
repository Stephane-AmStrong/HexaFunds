import { ErrorViewModel } from "./error-message/error-view-model";

export const ERROR_CATALOG: Record<string, ErrorViewModel> = {
  '401': {
    title: 'Unauthorized',
    message: 'You must sign in to access this resource.',
    imageUrl: 'img/401-unauthorized.svg',
    actionLabel: 'Sign In',
    actionUrl: '/login',
  },
  '403': {
    title: 'Access Denied',
    message: 'You do not have permission to access this resource.',
    imageUrl: 'img/403-forbidden.svg',
    actionLabel: 'Go Back',
    actionUrl: '/',
  },
  '404': {
    title: 'Resource Not Found',
    message: 'The resource you requested could not be found on the server.',
    imageUrl: 'img/404-resource-not-found.svg',
    actionLabel: 'Go Back',
    actionUrl: '/',
  },
  '429': {
    title: 'Too Many Requests',
    message: 'You have sent too many requests. Please wait before trying again.',
    imageUrl: 'img/429-too-many-requests.svg',
    actionLabel: 'Go to Dashboard',
    actionUrl: '/dashboard',
  },
  '500': {
    title: 'Internal Server Error',
    message: 'An internal server error occurred. Please try again later.',
    imageUrl: 'img/500-internal-server-error.svg',
    actionLabel: 'Return Home',
    actionUrl: '/dashboard',
  },
  '502': {
    title: 'Bad Gateway',
    message: 'The server received an invalid response. Please try again shortly.',
    imageUrl: 'img/502-bad-gateway.svg',
    actionLabel: 'Refresh Page',
    actionUrl: '/dashboard',
  },
  '503': {
    title: 'Service Unavailable',
    message: 'The service is currently unavailable. Please try again shortly.',
    imageUrl: 'img/503-service-unavailable.svg',
    actionLabel: 'Refresh Page',
    actionUrl: '/dashboard',
  },
  'unknown': {
    title: 'Something Went Wrong',
    message: 'An unexpected error occurred. Please try again or contact support.',
    imageUrl: 'img/error-generic.svg',
    actionLabel: 'Go to Dashboard',
    actionUrl: '/dashboard',
  },
};

const ERROR_DYNAMIC_ROUTES: Record<number, string> = {
  401: '/error/unauthorized',
  403: '/error/forbidden',
  404: '/error/resource-not-found',
  500: '/error/internal-server-error',
  502: '/error/bad-gateway',
  503: '/error/service-unavailable',
  429: '/error/rate-limit',
};

const DEFAULT_ERROR_DYNAMIC_ROUTE = "/error/unknown";

export { ERROR_DYNAMIC_ROUTES, DEFAULT_ERROR_DYNAMIC_ROUTE };