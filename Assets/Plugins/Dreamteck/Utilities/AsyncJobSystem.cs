using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace Dreamteck
{
    public class AsyncJobSystem : MonoBehaviour
    {
        Queue<IJobData> _jobs = new Queue<IJobData>();

        IJobData _currentJob = null;

        bool _isWorking = false;

        public AsyncJobOperation ScheduleJob<T>(JobData<T> data)
        {
            _jobs.Enqueue(data);
            return new AsyncJobOperation(data);
        }

        void Update()
        {
            if (_jobs.Count > 0 && !_isWorking)
            {
                StartCoroutine(JobCoroutine());
            }
        }

        IEnumerator JobCoroutine()
        {
            _isWorking = true;
            
            while (_jobs.Count > 0)
            {
                _currentJob = _jobs.Dequeue();
                _currentJob.Initialize();

                while (!_currentJob.done)
                {
                    _currentJob.Next();
                    yield return null;
                }

                _currentJob.Complete();
                _currentJob = null;

                yield return null;
            }

            _isWorking = false;
        }


        public class AsyncJobOperation : CustomYieldInstruction
        {
            IJobData _job;
            
            public AsyncJobOperation(IJobData job)
            {
                _job = job;
            }

            public override bool keepWaiting {
                get { return !_job.done; }
            }
        }

        public interface IJobData
        {
            bool done { get; }

            void Initialize();

            void Next();

            void Complete();
        }

        public class JobData<T> : IJobData
        {
            int _index;

            int _iterations = 0;

            IEnumerable<T> _collection;

            Action<JobData<T>> _onComplete;

            Action<JobData<T>> _onIteration;

            IEnumerator<T> _enumerator;

            public T current { get { return _enumerator.Current; } }

            public int index  { get  { return _index; } }

            public IEnumerable<T> collection { get { return _collection; } }

            public bool done { get; private set; }

            public JobData(IEnumerable<T> collection, int iterations, Action<JobData<T>> onIteration)
            {
                _collection = collection;
                _onIteration = onIteration;
                _iterations = iterations;
                done = false;
            }

            public JobData(IEnumerable<T> collection, int iterations, Action<JobData<T>> onIteration, Action<JobData<T>> onComplete) :
                this(collection, iterations, onIteration)
            {
                _onComplete = onComplete;
            }

            public void Initialize()
            {
                _enumerator = _collection.GetEnumerator();
                _index = -1;
                done = !_enumerator.MoveNext();
            }

            public void Complete()
            {
                _enumerator.Dispose();

                try
                {
                    if (_onComplete != null) {
                        _onComplete(this);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            public void Next()
            {
                int counter = _iterations;

                if (done)
                {
                    return;
                }
                do
                {
                    _index++;

                    try
                    {
                        if(_onIteration != null)
                        {
                            _onIteration(this);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    done = !_enumerator.MoveNext();
                }
                while (!done && --counter > 0);
            }
        }
    }
}